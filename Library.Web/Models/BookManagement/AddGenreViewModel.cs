using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.BookManagement
{
    public class AddGenreViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
