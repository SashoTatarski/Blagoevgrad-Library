using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Utils;
using Library.Services.Contracts;
using Library.Web.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly ILibrarySystem _system;

        public UserAccountController(IAccountManager accountManager, ILibrarySystem system)
        {
            _accountManager = accountManager;
            _system = system;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            var vm = user.MapToViewModel();

            return View(vm);
        }

        public async Task<IActionResult> CancelAccount(string id)
        {
            bool hasOverdueBooks = await _system.HasOverdueBooks(id);
            if (hasOverdueBooks)
            {
                TempData["message"] = Constants.AcctCancelRetBks;
                return RedirectToAction("Index", "UserBooks");
            }

            await _system.AccountCancel(id);
            TempData["message"] = Constants.AcctCancel;

            return RedirectToAction("Logout", "Auth");
        }
    }
}