using Fomo.Application.DTO;
using Fomo.Domain.Entities;
using Fomo.Infrastructure.Persistence;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fomo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly EFCoreDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public UserController (EFCoreDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create()
        {
            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var picture = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            
            if (auth0Id == null || name == null || email == null || picture == null)
            {
                return BadRequest("Cannot obtain user info");
            }

            var newUser = new User()
            {
                Auth0Id = auth0Id,
                Name = name,
                Email = email,
                ProfilePictureUrl = picture
            };

            await _userRepository.InsertAsync(newUser);
            await _userRepository.SaveAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("details")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details()
        {
            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return BadRequest("Invalid UserId");
            }

            var userdata = await _userRepository.GetByAuth0IdAsync(auth0Id);

            if (userdata == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO()
            {
                Name = userdata.Name,
                Email = userdata.Email,
                ProfilePictureUrl = userdata.ProfilePictureUrl,
                SmaAlert = userdata.SmaAlert,
                BollingerAlert = userdata.BollingerAlert,
                StochasticAlert = userdata.StochasticAlert,
                RsiAlert = userdata.RsiAlert,
                TradeResults = userdata.TradeResults
            };

            return Ok(userDto);
        }

        [Authorize]
        [HttpPatch("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromBody] UserDTO userUpdate)
        {
            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return BadRequest("Invalid UserId");
            }

            var userData = await _userRepository.GetByAuth0IdAsync(auth0Id);

            if (userData == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(userUpdate.Name)) userData.Name = userUpdate.Name;
            if (userUpdate.ProfilePictureUrl != null) userData.ProfilePictureUrl = userUpdate.ProfilePictureUrl;
            if (userUpdate.SmaAlert.HasValue) userData.SmaAlert = userUpdate.SmaAlert.Value;
            if (userUpdate.BollingerAlert.HasValue) userData.BollingerAlert = userUpdate.BollingerAlert.Value;
            if (userUpdate.StochasticAlert.HasValue) userData.StochasticAlert = userUpdate.StochasticAlert.Value;
            if (userUpdate.RsiAlert.HasValue) userData.RsiAlert = userUpdate.RsiAlert.Value;

            await _userRepository.UpdateAsync(userData);
            await _userRepository.SaveAsync();

            return Ok();
        }

        [Authorize]
        [HttpPatch("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {
            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return BadRequest("Invalid UserId");
            }

            await _userRepository.DeleteAsync(auth0Id);
            await _userRepository.SaveAsync();

            return Ok();
        }
    }
}
