using Library.Models.Enums;
using Library.Models.Models;
using Library.Web.Models.BookManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.AccountManagement
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public string UserId { get; set; }

        public DateTime MembershipStartDate { get; set; }

        public DateTime MembershipEndDate { get; set; }

        public double Wallet { get; set; }

        public AccountStatus Status { get; set; }

        public List<BookIssuedViewModel> CheckedoutBooks { get; set; }

        public List<BookIssuedViewModel> ReservedBooks { get; set; }

        public BookIssuedViewModel Rating { get; set; }

        public string BanDescription { get; set; }

    }
}
