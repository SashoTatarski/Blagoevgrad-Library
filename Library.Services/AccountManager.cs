using Library.Database;
using Library.Models.Enums;
using Library.Models.Models;
using Library.Models.Utils;
using Library.Services.Contracts;
using Library.Services.HashProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly LibraryContext _context;
        private readonly IHasher _hasher;
        

        public AccountManager(LibraryContext context, IHasher hasher)
        {
            _context = context;
            _hasher = hasher;            
        }
        public async Task<User> GetUserByIdAsync(string id)
            => await _context.Users
            .Include(u => u.ReservedBooks)
                .ThenInclude(ch => ch.Book)
                    .ThenInclude(x => x.Author)
            .Include(u => u.CheckedoutBooks)
                .ThenInclude(ch => ch.Book)
                    .ThenInclude(x => x.Author)
            .Include(u => u.Ratings)
            .FirstOrDefaultAsync(u => u.Id.ToString() == id)
            .ConfigureAwait(false);

        public async Task<User> GetUserByUsernameAsync(string username)
            => await _context.Users
            .Include(u => u.ReservedBooks)
                .ThenInclude(x => x.Book)
                    .ThenInclude(x => x.Author)
            .Include(u => u.CheckedoutBooks)
                 .ThenInclude(x => x.Book)
                    .ThenInclude(x => x.Author)
            .Include(u => u.Ratings)
            .Include(u => u.Notifications)
            .FirstOrDefaultAsync(u => u.Username == username)
            .ConfigureAwait(false);

        public async Task<User> ActivateUserAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id).ConfigureAwait(false);
            user.Status = AccountStatus.Active;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id).ConfigureAwait(false);

            user.Status = AccountStatus.Inactive;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        // TODO fix banned users
        public async Task BanUserAsync(string id)
        {
            var user = await _context.Users.Where(u => u.Id.ToString() == id).FirstOrDefaultAsync().ConfigureAwait(false);

            user.Status = AccountStatus.Banned;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await this.CreateBannedUserAsync(user);
        }

        private async Task CreateBannedUserAsync(User user)
        {
            var bannedUser = new BannedUser()
            {
                UserId = user.Id,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30)
                //Description = ...
            };

            _context.BannedUsers.Add(bannedUser);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void CheckStatus(User user)
        {
            if (user.Status == AccountStatus.Banned)
            {
                throw new ArgumentException(Constants.UserIsBanned);
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new ArgumentException(Constants.AcctDeactivated);
            }
        }

        public async Task<List<User>> GetAllUsersAsync() => await _context.Users
            .Include(u => u.ReservedBooks)
                .ThenInclude(ch => ch.Book)
                    .ThenInclude(x => x.Author)
            .Include(u => u.CheckedoutBooks)
                .ThenInclude(ch => ch.Book)
                    .ThenInclude(x => x.Author)
            .ToListAsync().ConfigureAwait(false);


        public async Task<User> CreateAsync(string username, string password, int membershipMonths)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                throw new ArgumentException($"{username} is already taken");
            }

            var defaultRole = _context.Roles.FirstOrDefault(r => r.RoleName == Constants.DefaultRole);

            if (defaultRole is null)
            {
                throw new InvalidOperationException(Constants.NoDefRoleDb);
            }

            var user = new User
            {
                Username = username,
                HashPassword = _hasher.Hash(password),
                Role = defaultRole,
                MembershipStartDate = DateTime.Today,
                MembershipEndDate = DateTime.Today.AddMonths(membershipMonths),
                Status = AccountStatus.Active,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public User Find(string username, string password)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username);

            if (user is null || !_hasher.Verify(password, user.HashPassword))
            {
                throw new ArgumentException(Constants.UsernamePassIncorr);
            }           

            return user;
        }

        public async Task<User> GetAdminAccountAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Role.RoleName == "admin")
                .ConfigureAwait(false);
        }
    }
}
