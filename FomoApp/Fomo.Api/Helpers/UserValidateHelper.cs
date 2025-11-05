using Fomo.Domain.Entities;
using Fomo.Infrastructure.Repositories;
using System.Security.Claims;

namespace Fomo.Api.Helpers
{
    public class UserValidateHelper : IUserValidateHelper
    {
        private readonly IUserRepository _userRepository;

        public UserValidateHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetFullUserAsync(ClaimsPrincipal user)
        {
            var auth0Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return null;
            }

            return await _userRepository.GetByAuth0IdAsync(auth0Id);
        }

        public async Task<User?> GetOnlyUserAsync(ClaimsPrincipal user)
        {
            var auth0Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return null;
            }

            return await _userRepository.GetOnlyUserByAuth0IdAsync(auth0Id);
        }

        public async Task<User?> GetUserIdAsync(ClaimsPrincipal user)
        {
            var auth0Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return null;
            }

            return await _userRepository.GetUserIdByAuth0IdAsync(auth0Id);
        }
    }
}
