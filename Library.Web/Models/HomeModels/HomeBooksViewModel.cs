using Library.Web.Models.BookViewModels;
using System.Collections.Generic;

namespace Library.Web.Models.HomeModels
{
    public class HomeBooksViewModel
    {
        public List<GenericBookViewModel> Books { get; set; }
    }
}
