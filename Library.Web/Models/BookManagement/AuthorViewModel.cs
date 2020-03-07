using System.Collections.Generic;

namespace Library.Web.Models.BookManagement
{
    public class AuthorViewModel
    {
        public string AuthorName { get; set; }
        public string Id { get; set; }

        public List<BookViewModel> Books { get; set; }
    }
}
