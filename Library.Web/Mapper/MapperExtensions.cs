using Library.Models.Models;
using Library.Web.Models.AccountManagement;
using Library.Web.Models.BookManagement;
using Library.Web.Models.BookViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Web.Mapper
{
    public static class MapperExtensions
    {
        public static BookViewModel MapToViewModel(this Book book)
        {
            var vm = new BookViewModel();

            vm.BookId = book.Id.ToString();
            vm.Title = book.Title;
            vm.Year = book.Year;
            vm.Rack = book.Rack;
            vm.ISBN = book.ISBN;
            vm.Rating = String.Format("{0:0.00}", book.Rating);
            vm.Author = book.Author;
            vm.Publisher = book.Publisher;
            vm.Status = book.Status;
            vm.Genres = book.BookGenres.Select(bg => bg.Genre).ToList();

            vm.AllBookCopies = new List<BookCopyViewModel>();

            return vm;
        }

        public static BookCopyViewModel MapToCopyViewModel(this Book book)
        {
            var vm = new BookCopyViewModel();
            vm.Title = book.Title;
            vm.Status = book.Status.ToString();
            vm.Isbn = book.ISBN;

            if (book.ReservedBooks != null)
            {
                var resUsers = new List<User>();
                book.ReservedBooks.ForEach(x => resUsers.Add(x.User));
                vm.ReservedBy = resUsers;
            }

            if (book.CheckedoutBook != null)
            {
                vm.CheckedOutBy = book.CheckedoutBook.User;
            }

            return vm;
        }

        public static GenericBookViewModel MapToGenericViewModel(this Book book)
        {
            var genresAsStrings = new List<string>();
            book.BookGenres.ForEach(bg => genresAsStrings.Add(bg.Genre.Name));
            
            var vm = new GenericBookViewModel()
            {
                Title = book.Title,
                AuthorName = book.Author.Name,
                Genres = genresAsStrings,
                ISBN = book.ISBN,
                Publisher = book.Publisher.Name,
                Rating = String.Format("{0:0.0}", book.Rating),
                Year = book.Year,
            };

            return vm;
        }

        public static BookIssuedViewModel MapToViewModel(this CheckoutBook book)
        {
            var selectItems = new List<SelectListItem>();
            for (int i = 1; i <= 10; i++)
            {
                selectItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            var vm = new BookIssuedViewModel();

            vm.BookId = book.BookId.ToString();
            vm.Author = book.Book.Author;
            vm.StartDate = book.CheckoutDate;
            vm.EndDate = book.DueDate;
            vm.ISBN = book.Book.ISBN;
            vm.Title = book.Book.Title;
            vm.Status = book.Book.Status.ToString();
            vm.IsReserved = false;
            vm.RatingList = selectItems;

            return vm;
        }

        public static BookIssuedViewModel MapToViewModel(this ReservedBook book)
        {
            var selectItems = new List<SelectListItem>();
            for (int i = 1; i <= 10; i++)
            {
                selectItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            var vm = new BookIssuedViewModel();

            vm.BookId = book.BookId.ToString();
            vm.Author = book.Book.Author;
            vm.StartDate = book.ReservationDate;
            vm.EndDate = book.ReservationDueDate;
            vm.ISBN = book.Book.ISBN;
            vm.Title = book.Book.Title;
            vm.Status = book.Book.Status.ToString();
            vm.IsReserved = true;
            vm.RatingList = selectItems;

            return vm;
        }

        public static UserViewModel MapToViewModel(this User user)
        {
            var vm = new UserViewModel();
            vm.UserId = user.Id.ToString();
            vm.Username = user.Username;
            vm.MembershipStartDate = user.MembershipStartDate;
            vm.MembershipEndDate = user.MembershipEndDate;
            vm.Status = user.Status;
            vm.Wallet = user.Wallet;

            if (user.CheckedoutBooks != null)
                vm.CheckedoutBooks = new List<BookIssuedViewModel>(user.CheckedoutBooks.Select(x => x.MapToViewModel()).ToList());

            if (user.ReservedBooks != null)
                vm.ReservedBooks = new List<BookIssuedViewModel>(user.ReservedBooks.Select(x => x.MapToViewModel()).ToList());

            return vm;
        }
    }
}
