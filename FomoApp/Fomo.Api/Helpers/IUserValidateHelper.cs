using Fomo.Domain.Entities;
using System.Security.Claims;

namespace Fomo.Api.Helpers
{
    public interface IUserValidateHelper
    {
        Task<User?> GetFullUserAsync(ClaimsPrincipal user);

        Task<User?> GetOnlyUserAsync(ClaimsPrincipal user);

        Task<User?> GetUserIdAsync(ClaimsPrincipal user);
    }
}
