using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string ISBN { get; set; }

        [Required]
        [Range(1629, 2019)]
        public int Year { get; set; }   
        
        [Range(1, 1000)]
        public int Rack { get; set; }

        [Required]
        public BookStatus Status { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public double Rating { get; set; }
        public bool IsLocked { get; set; }

        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public List<BookGenre> BookGenres { get; set; }
        public CheckoutBook CheckedoutBook { get; set; }
        public List<ReservedBook> ReservedBooks { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
