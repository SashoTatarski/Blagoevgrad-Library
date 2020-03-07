using Library.Database;
using Library.Models.Enums;
using Library.Models.Models;
using Library.Models.Utils;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LibrarySystem : ILibrarySystem
    {
        private readonly IBookManager _bookManager;
        private readonly IAccountManager _accountManager;
        private readonly LibraryContext _context;


        public LibrarySystem(IBookManager bookManager, IAccountManager accountManager, LibraryContext context)
        {
            _bookManager = bookManager;
            _accountManager = accountManager;
            _context = context;
        }

        public async Task RateBook(string userName, string isbn, string newRating)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);
            var calculatedRating = await RecalculateRating(isbn, int.Parse(newRating));

            var books = await _bookManager.GetBooksByIsbnAsync(isbn);
            foreach (var book in books)
            {
                var rating = new Rating
                {
                    BookId = book.Id,
                    UserId = user.Id,
                    Rate = int.Parse(newRating)
                };
                _context.Ratings.Add(rating);
                book.Rating = calculatedRating;
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsBookRatedByUser(string isbn, string userId)
        {
            return await _context.Ratings
                 .Include(x => x.Book)
                 .AnyAsync(x => x.UserId.ToString() == userId && x.Book.ISBN == isbn)
                 .ConfigureAwait(false);
        }

        public async Task<double> RecalculateRating(string isbn, int newRating)
        {
            var bookRatings = await _context.Ratings
                .Include(x => x.Book)
                .Where(x => x.Book.ISBN == isbn)
                .GroupBy(x => x.UserId)
                .Select(x => x.First())
                .ToListAsync()
                .ConfigureAwait(false);

            return (double)(bookRatings.Sum(x => x.Rate) + newRating) / (double)(bookRatings.Count() + 1);
        }

        public async Task MarkNotificationSeen(string notifId)
        {
            var notif = await _context.Notifications
                 .FirstOrDefaultAsync(n => n.Id.ToString() == notifId)
                 .ConfigureAwait(false);
            notif.IsSeen = true;

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                 .Include(x => x.User)
                 .ToListAsync()
                 .ConfigureAwait(false);
        }
        public bool IsBookCheckedout(User user, string isbn) => user.CheckedoutBooks.Any(x => x.Book.ISBN == isbn);

        public bool IsMaxCheckedoutQuota(User user) => user.CheckedoutBooks.Count >= Constants.MaxBookQuota;

        public async Task AccountCancel(string id)
        {
            bool hasOverdueBooks = await this.HasOverdueBooks(id);
            if (hasOverdueBooks)
                throw new Exception(Constants.AcctCancelRetBks);

            var user = await _accountManager.GetUserByIdAsync(id);

            for (var i = 0; user.CheckedoutBooks.Count > 0;)
            {
                await this.ReturnCheckedBookAsync(user.Username, user.CheckedoutBooks[i].BookId.ToString());
            }

            for (var i = 0; user.ReservedBooks.Count > 0;)
            {
                await this.ReturnResBookAsync(user.Username, user.ReservedBooks[i].BookId.ToString());
            }

            //foreach (var book in user.CheckedoutBooks)
            //{
            //    await this.ReturnCheckedBookAsync(user.Username, book.BookId.ToString());
            //}

            await _accountManager.DeleteUserAsync(id);

            var message = string.Format(Constants.CancelMembershipNotif, user.Username);
            await this.AddNotificationAsync(message, user);
        }

        public async Task<bool> HasOverdueBooks(string id)
        {
            var user = await _accountManager.GetUserByIdAsync(id);

            return (from book in user.CheckedoutBooks
                    where book.DueDate < DateTime.Today
                    select book).Any();
        }

        public async Task<List<Book>> GetCheckeoutBooksAsync(string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);

            var checkedoutBooks = await _context.CheckoutBooks.Where(u => u.UserId == user.Id).ToListAsync().ConfigureAwait(false);

            var allBooks = await _bookManager.GetAllBooksAsync();

            List<Book> booksToReturn = new List<Book>();
            foreach (var book in allBooks)
            {
                foreach (var chBook in checkedoutBooks)
                {
                    if (book.Id == chBook.BookId)
                        booksToReturn.Add(book);
                }
            }

            return booksToReturn;
        }

        public async Task<List<Book>> GetReservedBooksAsync(string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);

            var reservedBooks = await _context.ReservedBooks.Where(u => u.UserId == user.Id).ToListAsync().ConfigureAwait(false);

            var allBooks = await _bookManager.GetAllBooksAsync();

            List<Book> booksToReturn = new List<Book>();
            foreach (var book in allBooks)
            {
                foreach (var chBook in reservedBooks)
                {
                    if (book.Id == chBook.BookId)
                        booksToReturn.Add(book);
                }
            }

            return booksToReturn;
        }

        public async Task<string> ReturnCheckedBookAsync(string isbn, string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);
            var bookToReturn = user.CheckedoutBooks.FirstOrDefault(b => b.Book.ISBN == isbn);

            if (bookToReturn is null)
            {
                throw new ArgumentException(Constants.NotCheckedOutThisBook);
            }

            await this.ChangeBookStatusAsync(bookToReturn.Book.Id.ToString(), BookStatus.Available);

            _context.CheckoutBooks.Remove(bookToReturn);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            await this.IfUserRestrictedWhenReturning(user);
            var message = string.Format(Constants.ReturnBookNotification, user.Username, bookToReturn.Book.Title, bookToReturn.BookId);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());

            return Constants.ResBookSucc;
        }

        private async Task IfUserRestrictedWhenReturning(User user)
        {
            if (user.Status == AccountStatus.Restricted)
            {
                var overdueBooks = await this.GetUserOverdueBooks(user);
                if (overdueBooks.Count == 0)
                {
                    user.Status = AccountStatus.Active;
                }
            }
        }

        private async Task<List<CheckoutBook>> GetUserOverdueBooks(User user)
        {
            return await _context.CheckoutBooks
                .Where(b => b.UserId == user.Id && b.DueDate < DateTime.Today)
                .ToListAsync();
        }

        public async Task ReturnResBookAsync(string userName, string bookId)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);

            var resBook = await _context.ReservedBooks
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.UserId == user.Id && b.BookId.ToString() == bookId).ConfigureAwait(false);

            _context.ReservedBooks.Remove(resBook);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await this.ChangeStatusCancelReservation(resBook.BookId.ToString());

            var message = string.Format(Constants.ReturnBookNotification, user.Username, resBook.Book.Title, resBook.BookId);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());
        }

        public async Task<bool> AreAllCopiesCheckedAsync(string isbn)
        {
            var books = await _bookManager.GetBooksByIsbnAsync(isbn);

            foreach (var book in books)
            {
                if (book.Status == BookStatus.Available || book.Status == BookStatus.Reserved)
                    return false;
            }

            return true;
        }

        public async Task AddBookToReservedBooksAsync(string isbn, string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);
            var bookToReserve = await this.ReserveTheMostSuitableBookAsync(isbn);

            if (bookToReserve is null)
            {
                throw new ArgumentException(Constants.BookToBeDeleted);
            }

            var newBook = new ReservedBook()
            {
                BookId = bookToReserve.Id,
                UserId = user.Id,
                ReservationDate = null,
                ReservationDueDate = null
            };

            await this.ChangeBookStatusAsync(bookToReserve.Id.ToString(), BookStatus.Reserved);

            _context.ReservedBooks.Add(newBook);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var message = string.Format(Constants.ReserveBookNotification, user.Username, bookToReserve.Title, bookToReserve.Id);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());
        }

        public async Task AddBookToCheckoutBooksAsync(string isbn, string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);
            var booksByIsbn = await _bookManager.GetBooksByIsbnAsync(isbn);

            var bookToTake = booksByIsbn.FirstOrDefault(b => b.Status == BookStatus.Available);

            if (bookToTake is null)
            {
                throw new ArgumentException(Constants.NoAvailableBooks);
            }

            var newBook = new CheckoutBook()
            {
                BookId = bookToTake.Id,
                UserId = user.Id,
                CheckoutDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(Constants.MaxCheckoutDays)
            };

            await this.ChangeBookStatusAsync(bookToTake.Id.ToString(), BookStatus.CheckedOut);

            _context.CheckoutBooks.Add(newBook);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var message = string.Format(Constants.CheckoutBookNotification, user.Username, newBook.Book.Title, newBook.BookId);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());
        }

        private async Task ChangeStatusCancelReservation(string bookId)
        {
            var book = await _bookManager.GetBookByIdAsync(bookId);
            if (book.Status == BookStatus.Reserved)
            {
                book.Status = BookStatus.Available;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            else if (book.Status == BookStatus.CheckedOutAndReserved)
            {
                var otherReservations = await _context.ReservedBooks.Where(b => b.BookId.ToString() == bookId).ToListAsync().ConfigureAwait(false);
                if (otherReservations.Count == 0)
                {
                    book.Status = BookStatus.CheckedOut;
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
        private async Task ChangeBookStatusAsync(string bookId, BookStatus status)
        {
            var book = await _bookManager.GetBookByIdAsync(bookId);

            if (book.Status == BookStatus.CheckedOut && status == BookStatus.Reserved)
                book.Status = BookStatus.CheckedOutAndReserved;
            else if ((book.Status == BookStatus.CheckedOutAndReserved || book.Status == BookStatus.Reserved) && status == BookStatus.Reserved)
            { }
            else if (book.Status == BookStatus.CheckedOutAndReserved)
                book.Status = BookStatus.Reserved;
            else
                book.Status = status;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string> ExtendBookDueDate(string isbn, string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);
            var bookToExtend = user.CheckedoutBooks.FirstOrDefault(b => b.Book.ISBN == isbn);

            if (user.Wallet - Constants.ExtendCost < 0)
            {
                return Constants.NotEnoughMoney;
            }

            var newDueDate = bookToExtend.DueDate.AddDays(Constants.ExtendPeriod);

            if (newDueDate > user.MembershipEndDate)
            {
                return Constants.MembershipExpirationWarning;
            }

            user.Wallet -= Constants.ExtendCost;
            bookToExtend.DueDate = newDueDate;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            var message = string.Format(Constants.ExtendBookNotification, user.Username, bookToExtend.Book.Title, bookToExtend.BookId);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());

            return Constants.ExtendSuccess;

        }

        public async Task<string> CancelReservation(string id, string userName)
        {
            var user = await _accountManager.GetUserByUsernameAsync(userName);

            var reservatedBookToCancel = await _context.ReservedBooks
                .Include(rb => rb.Book)
                .FirstOrDefaultAsync(resBook => resBook.BookId.ToString() == id && resBook.UserId == user.Id)
                .ConfigureAwait(false);

            _context.ReservedBooks.Remove(reservatedBookToCancel);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var message = string.Format(Constants.CancelReservationNotification, user.Username, reservatedBookToCancel.Book.Title, reservatedBookToCancel.BookId);
            await this.AddNotificationAsync(message, await _accountManager.GetAdminAccountAsync());

            return Constants.CancelReservationSuccess;
        }

        public async Task AddNotificationAsync(string message, User user)
        {
            var notification = new Notification
            {
                SentOn = DateTime.Now,
                IsSeen = false,
                Message = message,
                UserId = user.Id
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CheckForOverdueBooks()
        {
            var overdueBooks = await _context.CheckoutBooks
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.DueDate < DateTime.Today)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var book in overdueBooks)
            {
                book.User.Status = AccountStatus.Restricted;

                if (!HasNotificationAboutThisBookToday(book.User, book.Book.Title))
                {
                    var notification = string.Format(Constants.OverDueBookNotification, book.Book.Title, (DateTime.Today - book.DueDate).TotalDays);
                    await this.AddNotificationAsync(notification, book.User);
                }
            }
        }

        private bool HasNotificationAboutThisBookToday(User user, string title)
        {
            return _context.Notifications.Any(n => n.UserId == user.Id && n.SentOn.Date == DateTime.Today && n.Message.Contains(title));
        }

        public async Task CheckForOverdueMemberships()
        {
            var usersWithOverdueMembership = await _context.Users
                .Where(u => u.MembershipEndDate < DateTime.Today && u.Status != AccountStatus.Restricted)
                .ToListAsync();

            foreach (var user in usersWithOverdueMembership)
            {
                user.Status = AccountStatus.Restricted;

                var notification = string.Format(Constants.OverDueMembershipNotification, user.MembershipEndDate.ToShortDateString());
                await this.AddNotificationAsync(notification, user);
            }
        }

        public async Task CheckForOverdueReservations()
        {
            var overdueReservations = await _context.ReservedBooks
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.ReservationDueDate < DateTime.Today)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var book in overdueReservations)
            {
                _context.ReservedBooks.Remove(book);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                await this.ChangeStatusCancelReservation(book.BookId.ToString());

                var notification = string.Format(Constants.OverdueReservation, book.Book.Title);
                await this.AddNotificationAsync(notification, book.User);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        public async Task CheckForSoonOverdueMemberships()
        {
            var usersWithOverdueMembershipAfterThreeDays = await _context.Users
                .Where(u => u.MembershipEndDate == DateTime.Today.AddDays(3))
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var user in usersWithOverdueMembershipAfterThreeDays)
            {
                var notification = string.Format(Constants.OverDueMembershipAfterThreeDaysNotification);

                if (!_context.Notifications.Any(n => n.Message == notification && n.SentOn == DateTime.Today))
                {
                    await this.AddNotificationAsync(notification, user);
                }
            }

            var usersWithOverdueMembershipAfterTwoDays = await _context.Users
               .Where(u => u.MembershipEndDate == DateTime.Today.AddDays(2))
               .ToListAsync()
               .ConfigureAwait(false);

            foreach (var user in usersWithOverdueMembershipAfterTwoDays)
            {
                var notification = string.Format(Constants.OverDueMembershipAfterTwoDaysNotification);
                if (!_context.Notifications.Any(n => n.Message == notification && n.SentOn == DateTime.Today))
                {
                    await this.AddNotificationAsync(notification, user);
                }
            }

            var usersWithOverdueMembershipAfterOneDay = await _context.Users
               .Where(u => u.MembershipEndDate == DateTime.Today.AddDays(1))
               .ToListAsync()
               .ConfigureAwait(false);

            foreach (var user in usersWithOverdueMembershipAfterOneDay)
            {
                var notification = string.Format(Constants.OverDueMembershipAfterOneDayNotification);
                if (!_context.Notifications.Any(n => n.Message == notification && n.SentOn == DateTime.Today))
                {
                    await this.AddNotificationAsync(notification, user);
                }
            }
        }

        private async Task<Book> ReserveTheMostSuitableBookAsync(string isbn)
        {
            var allbooks = await _bookManager.GetBooksByIsbnAsync(isbn);

            var availableBook = allbooks.FirstOrDefault(b => b.Status == BookStatus.Available);
            if (availableBook != null)
            {
                return availableBook;
            }

            var onlyCheckedoutBook = allbooks.FirstOrDefault(b => b.Status == BookStatus.CheckedOut);
            if (onlyCheckedoutBook != null)
            {
                return onlyCheckedoutBook;
            }

            var checkedoutAndReservedBooks = allbooks.Where(b => b.Status == BookStatus.CheckedOutAndReserved).ToList();

            if (checkedoutAndReservedBooks.Count > 0)
            {
                var bookToReserve = checkedoutAndReservedBooks[0];

                foreach (var book in checkedoutAndReservedBooks)
                {
                    if (bookToReserve.ReservedBooks.Count > book.ReservedBooks.Count)
                    {
                        bookToReserve = book;
                    }
                }

                return bookToReserve;
            }
            else return null;
        }
    }
}

