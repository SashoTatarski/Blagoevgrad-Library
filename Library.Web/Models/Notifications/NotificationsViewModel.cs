using Library.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.Notifications
{
    public class NotificationsViewModel
    {
        public string Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public bool IsSeen { get; set; }

        public DateTime DateSent { get; set; }        
    }
}
