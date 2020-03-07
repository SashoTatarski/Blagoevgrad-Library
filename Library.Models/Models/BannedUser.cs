using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Models.Models
{
    public class BannedUser
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [StringLength(300)]
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
