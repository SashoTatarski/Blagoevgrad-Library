using Library.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required")]
        [MinLength(5, ErrorMessage = Constants.UsernameValid)]
        [Display(Name="Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(5, ErrorMessage = Constants.PasswordValid)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
    }
}
