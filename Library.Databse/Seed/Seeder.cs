using Library.Models.Enums;
using Library.Models.Models;
using System;
using System.Linq;

namespace Library.Database.Seed
{
    public static class Seeder
    {
        public static void Init(LibraryContext context)
        {
            SeedRoles(context);
            SeedAdmin(context);
            SeedBooks(context);
        }

        private static void SeedBooks(LibraryContext context)
        {
            if (context.Books.Any())
                return;

            var newBook = new Book
            {
                Title = "12 Rules For Life",
                Author = new Author { Name = "Jordan Peterson" },
                Publisher = new Publisher { Name = "Random House of Canada" },
                ISBN = "345816021",
                Year = 2018,
                Rack = 11,
                Rating = 9.8
            };

            context.Books.Add(newBook);
            var newGenre = new Genre { Name = "Psychology" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "Maps of Meaning",
                Author = context.Authors.FirstOrDefault(x => x.Name == "Jordan Peterson"),
                Publisher = context.Publishers.FirstOrDefault(x => x.Name == "Random House of Canada"),
                ISBN = "B00BW54XVO",
                Year = 1997,
                Rack = 22,
                Rating = 7.3
            };

            context.Books.Add(newBook);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "Dangerous",
                Author = new Author { Name = "Milo Yiannaopoulos" },
                Publisher = new Publisher { Name = "Instagram" },
                ISBN = "069289344X",
                Year = 2007,
                Rack = 33,
                Rating = 6.1
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Political Humor" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "The Right Side of History",
                Author = new Author { Name = "Ben Shapiro" },
                Publisher = new Publisher { Name = "Broadside Publishing" },
                ISBN = "62857908",
                Year = 2019,
                Rack = 44,
                Rating = 4.0
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Religion" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "Tree: Essence of Healing",
                Author = new Author { Name = "Sue Lilly" },
                Publisher = new Publisher { Name = "Capall Bann Pub" },
                ISBN = "1861630816",
                Year = 2001,
                Rack = 55,
                Rating = 2.9
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Mysticism" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "The Rise of Superman: Decoding the Science of Ultimate Human Performance",
                Author = new Author { Name = "Steven Kotler" },
                Publisher = new Publisher { Name = "New Harvest" },
                ISBN = "9781477800836",
                Year = 2014,
                Rack = 66,
                Rating = 5.1
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Sports" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "The Sync Book 2",
                Author = new Author { Name = "Alan Abbadessa" } /*context.Authors.FirstOrDefault(x => x.Name == "Alan Abbadessa")*/,
                Publisher = new Publisher { Name = "Sync Book Press" },
                ISBN = "0615724892",
                Year = 2012,
                Rack = 88,
                Rating = 6.2
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Unexplained Mysteries" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "This Is a Call: The Life and Times of Dave Grohl",
                Author = new Author { Name = "Paul Brannigan" },
                Publisher = new Publisher { Name = "Harper" },
                ISBN = "0007391234",
                Year = 2012,
                Rack = 99,
                Rating = 7.8
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Rock Music" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "Scar Tissue",
                Author = new Author { Name = "Anthony Kiedis " },
                Publisher = new Publisher { Name = "Sphere" },
                ISBN = "9780751535662",
                Year = 2005,
                Rack = 100,
                Rating = 9.1
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Stage Actor Biographies" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });
            context.SaveChanges();
            //--------------------------------------------------------------------------------
            newBook = new Book
            {
                Title = "Acid For The Children - The autobiography of Flea, the Red Hot Chili Peppers legend",
                Author = new Author { Name = "Flea" },
                Publisher = new Publisher { Name = "Headline" },
                ISBN = "1472230817",
                Year = 2019,
                Rack = 110,
                Rating = 8.3
            };
            context.Books.Add(newBook);
            newGenre = new Genre { Name = "Stage Actor Biographies" };
            context.Genres.Add(newGenre);
            context.BookGenre.Add(new BookGenre
            {
                BookId = newBook.Id,
                GenreId = newGenre.Id
            });

            context.SaveChanges();
        }

        private static void SeedRoles(LibraryContext context)
        {
            if (context.Roles.Any())
                return;

            var roleNames = new[] { "user", "admin" };
            context.Roles.AddRange(
                roleNames.Select(name => new Role { RoleName = name })
            );
            context.SaveChanges();
        }

        private static void SeedAdmin(LibraryContext context)
        {
            // check if teams are already in the db
            if (context.Users.Any(u => u.Username == "admin"))
                return;

            var adminRole = context.Roles.Where(r => r.RoleName == "admin").FirstOrDefault();

            context.Users.Add(new User
            {
                Username = "admin",
                HashPassword = "M8GXPIckpkXfuYusx9e1xzZt4eTqCim2gtd5xmsHk4ds+BePHFMN9+DItyR7IiaJ",
                RoleId = adminRole.Id,
                MembershipStartDate = DateTime.Today,
                MembershipEndDate = DateTime.Today.AddYears(1),
                Status = AccountStatus.Active
            });

            context.SaveChanges();
        }
    }
}
