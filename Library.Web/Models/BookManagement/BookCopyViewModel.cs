using Library.Models.Models;
using System.Collections.Generic;

namespace Library.Web.Models.BookManagement
{
    public class BookCopyViewModel
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public string Isbn { get; set; }

        public User CheckedOutBy { get; set; }
        public List<User> ReservedBy { get; set; }
        public bool IsBookCheckedout { get; set; }
        public bool IsChBooksMaxQuota { get; set; }
        public bool AreAllCopiesChecked { get; set; }
    }
}
