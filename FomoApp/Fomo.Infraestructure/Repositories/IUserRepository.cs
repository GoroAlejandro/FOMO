using Fomo.Domain.Entities;

namespace Fomo.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByAuth0IdAsync(string auth0id);
        Task<User?> GetOnlyUserByAuth0IdAsync(string auth0id);
        Task<User?> GetUserIdByAuth0IdAsync(string auth0id);
        Task InsertAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string auth0id);
        Task SaveAsync();
    }
}
