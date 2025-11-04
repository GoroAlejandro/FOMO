using Fomo.Domain.Entities;
using System.Security.Claims;

namespace Fomo.Api.Helpers
{
    public interface IUserValidateHelper
    {
        Task<User?> GetAuthenticatedUserAsync(ClaimsPrincipal user);
    }
}
