using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string HashPassword { get; set; }

        public DateTime MembershipStartDate { get; set; }

        public DateTime MembershipEndDate { get; set; }

        public double Wallet { get; set; }
        public AccountStatus Status { get; set; }
        public int RoleId { get; set; }

        public List<CheckoutBook> CheckedoutBooks { get; set; }
        public List<ReservedBook> ReservedBooks { get; set; }        
        public Role Role { get; set; }
        public List<Notification> Notifications { get; set; }
        public BannedUser BannedUser { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}