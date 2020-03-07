using Library.Models.Enums;
using Library.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.AccountManagement
{
    public class AccountViewModel
    {
        public List<User> AllUsers { get; set; }

        public List<UserViewModel> User { get; set; } = new List<UserViewModel>();
    }
}
