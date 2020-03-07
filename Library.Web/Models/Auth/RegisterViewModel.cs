using Library.Models.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.Auth
{
    public class RegisterViewModel
    {
        public List<SelectListItem> MembershipOption { get; set; }

        public int MembershipType { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(5, ErrorMessage = Constants.UsernameValid)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(5, ErrorMessage = Constants.PasswordValid)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ComparePassword { get; set; }
    }
}
