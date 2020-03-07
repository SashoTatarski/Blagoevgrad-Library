using Library.Models.Enums;
using Library.Models.Models;
using Library.Models.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.BookManagement
{
    public class BookViewModel
    {
        public string BookId { get; set; }

        public List<BookCopyViewModel> AllBookCopies { get; set; }

        [Required(ErrorMessage = Constants.BookCopiesReqErr)]
        [Range(1, 100, ErrorMessage = Constants.BookCopiesReqRange)]
        public int BookCopies { get; set; }

        [Required(ErrorMessage = Constants.AuthorsReqErr)]
        public List<SelectListItem> Authors { get; set; }

        public string AuthorId { get; set; }
        public Author Author { get; set; }


        [Required(ErrorMessage = Constants.PublishsReqErr)]
        public string PublisherId { get; set; }
        public List<SelectListItem> Publishers { get; set; }
        public Publisher Publisher { get; set; }
        
        [Required(ErrorMessage = Constants.GenresReqErr)]
        public List<int> GenresIds { get; set; }
        public List<SelectListItem> GenresOptions { get; set; }
        public List<Genre> Genres { get; set; }

        [Required(ErrorMessage = Constants.TitleReqErr)]
        public string Title { get; set; }

        //TODO: Add verification that isbn is 13 letters
        [Required(ErrorMessage = Constants.IsbnReqErr)]
        public string ISBN { get; set; }


        [Required(ErrorMessage = Constants.YearReqErr)]
        [Range(1629, 2019)]
        public int Year { get; set; }

        public int Rack { get; set; }
        public string Rating { get; set; }
        public BookStatus Status { get; set; }
        public bool IsBookCheckedout { get; set; }
        public bool IsChBooksMaxQuota { get; set; }
        public bool AreAllCopiesChecked { get; set; }

        public string UserStatus { get; set; }
    }
}
