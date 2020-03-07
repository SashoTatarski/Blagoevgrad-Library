using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.Notifications
{
    public class ListNotificationsViewModel
    {
        public List<NotificationsViewModel> NotificationsList { get; set; } = new List<NotificationsViewModel>();
    }
}
