using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(300)]
        public string Message { get; set; }

        
        public DateTime SentOn { get; set; }
        public bool IsSeen { get; set; }

        public User User { get; set; }
    }
}
