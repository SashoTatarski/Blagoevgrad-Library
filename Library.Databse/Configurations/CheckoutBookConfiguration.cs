using Library.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Database.Configurations
{
    class CheckoutBookConfiguration : IEntityTypeConfiguration<CheckoutBook>
    {
        public void Configure(EntityTypeBuilder<CheckoutBook> builder)
        {
            builder
                .HasKey(rb => rb.BookId);

            builder
               .HasOne(b => b.Book)
               .WithOne(rb => rb.CheckedoutBook);

            builder
                .HasOne(b => b.User)
                .WithMany(u => u.CheckedoutBooks)
                .HasForeignKey(x => x.UserId);
        }
    }
}
