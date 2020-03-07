using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Models;
using Library.Services.Contracts;
using Library.Web.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly ILibrarySystem _system;

        public NotificationsController(IAccountManager accountManager, ILibrarySystem system)
        {
            _accountManager = accountManager;
            _system = system;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            var listVM = new ListNotificationsViewModel();

            if (User.IsInRole("admin"))
            {
                await NotificationsMapper(user, listVM);
            }
            else if (User.IsInRole("user"))
            {
                await NotificationsMapper(user, listVM);
            }

            listVM.NotificationsList = listVM.NotificationsList.OrderByDescending(n => n.DateSent).ToList();
            return View(listVM);
        }

        private async Task NotificationsMapper(User user, ListNotificationsViewModel listVM)
        {
            foreach (var notification in user.Notifications)
            {
                User username = null;
                if (User.IsInRole("admin"))
                {
                    string usernameFromNotif = notification.Message.Substring(0, notification.Message.IndexOf(" "));
                    username = await _accountManager.GetUserByUsernameAsync(usernameFromNotif);
                }

                var vm = new NotificationsViewModel();

                vm.Id = notification.Id.ToString();
                vm.Message = notification.Message;
                vm.UserId = notification.User.Id.ToString();
                vm.IsSeen = notification.IsSeen;
                vm.DateSent = notification.SentOn;
                if (User.IsInRole("admin"))
                    vm.User = username;

                listVM.NotificationsList.Add(vm);
            }
        }
                
        public async Task<IActionResult> MarkSeen(string id)
        {
            await _system.MarkNotificationSeen(id);

            return RedirectToAction("Index");
        }
    }
}