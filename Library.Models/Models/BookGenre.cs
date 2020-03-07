using System;

namespace Library.Models.Models
{
    public class BookGenre
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
