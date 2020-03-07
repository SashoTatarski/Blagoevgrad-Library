using Library.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Library.Database.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            try
            {
                builder
               .HasKey(rating => new { rating.BookId, rating.UserId });

                builder
                    .HasOne(rating => rating.Book)
                    .WithMany(book => book.Ratings)
                    .HasForeignKey(rating => rating.BookId);

                builder
                    .HasOne(rating => rating.User)
                    .WithMany(user => user.Ratings)
                    .HasForeignKey(rating => rating.UserId);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
           
        }
    }
}
