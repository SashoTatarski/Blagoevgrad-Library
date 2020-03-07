using Library.Database.Configurations;
using Library.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Database
{
    public class LibraryContext : DbContext
    {        
        public LibraryContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<ReservedBook> ReservedBooks { get; set; }
        public DbSet<CheckoutBook> CheckoutBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BannedUser> BannedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new ReservedBookConfiguration());
            modelBuilder.ApplyConfiguration(new CheckoutBookConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BannedUserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
