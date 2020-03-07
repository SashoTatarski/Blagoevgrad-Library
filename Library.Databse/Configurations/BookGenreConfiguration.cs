using Library.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Database.Configurations
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            builder
                .HasOne(bg => bg.Book)
                .WithMany(bg => bg.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            builder
                .HasOne(bg => bg.Genre)
                .WithMany(bg => bg.BookGenres)
                .HasForeignKey(bg => bg.GenreId);
        }
    }
}
