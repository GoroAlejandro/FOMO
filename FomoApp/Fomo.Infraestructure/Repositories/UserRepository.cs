using Fomo.Domain.Entities;
using Fomo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fomo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EFCoreDbContext _dbContext;

        public UserRepository (EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.TradeResults)
                .FirstOrDefaultAsync(o => o.Email == email);

            return user;
        }

        public async Task InsertAsync(User user)
        {
            bool exist = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
            if (!exist)
            {
                await _dbContext.Users.AddAsync(user);
            }
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
