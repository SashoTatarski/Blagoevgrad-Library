using System;

namespace Library.Models.Models
{
    public class CheckoutBook
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }

        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
