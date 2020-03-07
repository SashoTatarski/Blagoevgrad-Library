using Library.Models.Utils;
using Library.Services.Contracts;
using Library.Web.Mapper;
using Library.Web.Models.AccountManagement;
using Library.Web.Models.BookManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "user")]
    public class UserBooksController : Controller
    {
        private readonly ILibrarySystem _system;
        private readonly IAccountManager _accountManager;

        public UserBooksController(ILibrarySystem system, IAccountManager accountManager)
        {
            _system = system;
            _accountManager = accountManager;
        }

        public async Task<IActionResult> Index(UserViewModel vm)
        {
            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            vm.CheckedoutBooks = user.CheckedoutBooks.Select(x => x.MapToViewModel()).ToList();
            vm.ReservedBooks = user.ReservedBooks.Select(x => x.MapToViewModel()).ToList();

            foreach (var book in vm.CheckedoutBooks)
            {
                book.IsBookRatedByUser = await _system.IsBookRatedByUser(book.ISBN, user.Id.ToString());
                book.UserStatus = user.Status.ToString();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RateBook(BookIssuedViewModel vm)
        {
            await _system.RateBook(User.Identity.Name, vm.ISBN, vm.Rate);

            TempData["message"] = Constants.BookReview;

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnBook(string id)
        {
            TempData["message"] = await _system.ReturnCheckedBookAsync(id, User.Identity.Name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReserveBook(string id)
        {
            try
            {
                await _system.AddBookToReservedBooksAsync(id, User.Identity.Name);
                TempData["message"] = Constants.ResBookSucc;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CheckoutBook(string id)
        {
            try
            {
                await _system.AddBookToCheckoutBooksAsync(id, User.Identity.Name);
                TempData["message"] = Constants.ChBookSucc;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ExtendDueDate(string id)
        {
            TempData["message"] = await _system.ExtendBookDueDate(id, User.Identity.Name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CancelReservation(string id)
        {
            TempData["message"] = await _system.CancelReservation(id, User.Identity.Name);

            return RedirectToAction("Index");
        }
    }
}