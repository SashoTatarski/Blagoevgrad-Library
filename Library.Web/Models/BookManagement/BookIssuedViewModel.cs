using Library.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Library.Web.Models.BookManagement
{
    public class BookIssuedViewModel
    {
        public string BookId { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        public bool IsReserved { get; set; }

        public List<SelectListItem> RatingList { get; set; }

        public string Rate { get; set; }

        public bool IsBookRatedByUser { get; set; }
        public string UserStatus { get; set; }
    }
}
