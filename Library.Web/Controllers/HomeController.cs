using Library.Services.Contracts;
using Library.Web.Models;
using Library.Web.Models.HomeModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Library.Web.Mapper;
using Library.Web.Models.BookViewModels;
using System.Collections.Generic;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookManager _bookManager;
        private readonly ILibrarySystem _system;
        private readonly IAccountManager _accountManager;

        public HomeController(IBookManager bookManager, ILibrarySystem system, IAccountManager accountManager)
        {
            _bookManager = bookManager;
            _system = system;
            _accountManager = accountManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            var topRatedBooks = await _bookManager.GetTopRatedBooks(6);

            var vm = new HomeBooksViewModel()
            {
                Books = new List<GenericBookViewModel>()
            };

            topRatedBooks.ForEach(b => vm.Books.Add(b.MapToGenericViewModel()));

            if (user != null)
            {
                foreach (var book in vm.Books)
                {
                    book.IsBookCheckedout = _system.IsBookCheckedout(user, book.ISBN);
                    book.AreAllCopiesChecked = await _system.AreAllCopiesCheckedAsync(book.ISBN);
                    book.IsChBooksMaxQuota = _system.IsMaxCheckedoutQuota(user);
                    book.UserStatus = user.Status.ToString();
                }
            }

            return View(vm);
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
