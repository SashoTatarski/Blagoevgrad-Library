using System;

namespace Library.Models.Models
{
    public class ReservedBook
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }

        public DateTime? ReservationDate { get; set; }
        public DateTime? ReservationDueDate { get; set; }
    }
}
