using Library.Models.Utils;
using Library.Services.Contracts;
using Library.Web.Mapper;
using Library.Web.Models.AccountManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AccountManagementController : Controller
    {
        private readonly IAccountManager _accountManager;

        public AccountManagementController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public async Task<IActionResult> Index(AccountViewModel vm)
        {
            var users = await _accountManager.GetAllUsersAsync();

            var usersMapped = users
                .Select(u => u.MapToViewModel())
                .ToList();

            foreach (var user in usersMapped)
            {
                vm.User.Add(user);
            }

            return View(vm);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _accountManager.DeleteUserAsync(id);

            TempData["message"] = Constants.AcctDeact;

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Ban(string id)
        {
            await _accountManager.BanUserAsync(id);

            TempData["message"] = Constants.AcctBan;

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activate(string id)
        {
            var accountToActivate = await _accountManager.ActivateUserAsync(id);

            TempData["message"] = string.Format(Constants.AcctActivated, accountToActivate.Username);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _accountManager.GetUserByIdAsync(id);

            var vm = user.MapToViewModel();

            return View(vm);
        }
    }
}