using Fomo.Domain.Entities;

namespace Fomo.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task InsertAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string email);
        Task SaveAsync();
    }
}
