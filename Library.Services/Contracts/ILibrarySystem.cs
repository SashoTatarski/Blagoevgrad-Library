using Library.Models.Enums;
using Library.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Contracts
{
    public interface ILibrarySystem
    {
        Task AddBookToCheckoutBooksAsync(string bookId, string userName);
        bool IsBookCheckedout(User user, string isbn);
        Task<string> ReturnCheckedBookAsync(string isbn, string userName);
        Task ReturnResBookAsync(string userName, string bookId);
        Task<List<Book>> GetReservedBooksAsync(string userName);
        Task AccountCancel(string id);
        Task<bool> HasOverdueBooks(string id);

        bool IsMaxCheckedoutQuota(User user);
        Task AddBookToReservedBooksAsync(string isbn, string userName);

        Task<bool> AreAllCopiesCheckedAsync(string isbn);
        Task<string> ExtendBookDueDate(string isbn, string userName);
        Task<string> CancelReservation(string id, string userName);
        Task AddNotificationAsync(string message, User user);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task MarkNotificationSeen(string notifId);
        Task RateBook(string userName, string isbn, string newRating);
        Task<double> RecalculateRating(string isbn, int newRating);
        Task<bool> IsBookRatedByUser(string isbn, string userId);
        Task CheckForOverdueBooks();
        Task CheckForOverdueMemberships();
        Task CheckForSoonOverdueMemberships();
        Task CheckForOverdueReservations();
    }
}
