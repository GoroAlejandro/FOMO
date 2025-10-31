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

        public async Task<User?> GetByAuth0IdAsync(string auth0id)
        {
            var user = await _dbContext.Users
                .Include(u => u.TradeResults)
                .FirstOrDefaultAsync(u => u.Auth0Id == auth0id);

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

        public async Task DeleteAsync(string auth0id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0id);
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
