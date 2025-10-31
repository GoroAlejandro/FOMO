using Fomo.Application.DTO;
using Fomo.Domain.Entities;
using Fomo.Infrastructure.Persistence;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fomo.Api.Controllers
{
    [Route("api/[controller]")]
    public class TradeResultController : Controller
    {
        private readonly EFCoreDbContext _dbContext;
        private readonly ITradeResultRepository _tradeResultRepository;
        private readonly IUserRepository _userRepository;

        public TradeResultController(EFCoreDbContext dbContext, ITradeResultRepository tradeResultRepository,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _tradeResultRepository = tradeResultRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TradeResultDTO tradeResult)
        {
            if (tradeResult == null)
            {
                return BadRequest("Invalid tradeResult");
            }

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

            var newTradeResult = new TradeResult()
            {
                Symbol = tradeResult.Symbol,
                EntryPrice = tradeResult.EntryPrice,
                ExitPrice = tradeResult.ExitPrice,
                NumberOfStocks = tradeResult.NumberOfStocks,
                EntryDate = tradeResult.EntryDate,
                ExitDate = tradeResult.ExitDate,
                UserId = userData.UserId,
                TradeMethod = tradeResult.TradeMethod == null ? new TradeMethod() : new TradeMethod
                {
                    Sma = tradeResult.TradeMethod.Sma,
                    Bollinger = tradeResult.TradeMethod.Bollinger,
                    Stochastic = tradeResult.TradeMethod.Stochastic,
                    Rsi = tradeResult.TradeMethod.Rsi,
                    Other = tradeResult.TradeMethod.Other,
                }
            };

            if (IsInvalidTradeResult(newTradeResult))
            {
                return BadRequest("TradeResult data is invalid");
            }

            newTradeResult.CalculateProfit();
            await _tradeResultRepository.InsertAsync(newTradeResult);
            await _tradeResultRepository.SaveAsync();

            return Ok();
        }
                
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<TradeResultDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AllResults()
        {
            var tradeResults = await _tradeResultRepository.GetAllAsync();

            if (tradeResults == null)
            {
                return NotFound();
            }

            var tradeResultsDTO = tradeResults.Select(tr => new TradeResultDTO
            {
                Symbol = tr.Symbol,
                EntryPrice = tr.EntryPrice,
                ExitPrice = tr.ExitPrice,                
                Profit = tr.Profit,
                NumberOfStocks = tr.NumberOfStocks,
                EntryDate = tr.EntryDate,
                ExitDate = tr.ExitDate,
                TradeMethod = tr.TradeMethod,
                UserName = tr.User?.Name,
            });

            return Ok(tradeResultsDTO);
        }

        [Authorize]
        [HttpPatch("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromBody] TradeResultDTO Update)
        {
            if (Update == null)
            {
                return BadRequest("TradeResult cannot be null");
            }

            if (Update.TradeResultId == null)
            {
                return BadRequest("Id cannot be null");
            }

            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return BadRequest("Invalid UserId");
            }

            var userData = await _userRepository.GetByAuth0IdAsync(auth0Id);

            if (userData == null)
            {
                return NotFound("User not found");
            }

            var tradeResult = await _dbContext.TradeResults.FindAsync(Update.TradeResultId);

            if (tradeResult == null)
            {
                return BadRequest("Invalid TradeResultId");
            }

            if (tradeResult.UserId != userData.UserId)
            {
                return BadRequest("Only the creator can edit the post");
            }

            if (Update.Symbol != null && Update.Symbol != "") tradeResult.Symbol = Update.Symbol;
            if (Update.EntryPrice > 0) tradeResult.EntryPrice = Update.EntryPrice;
            if (Update.ExitPrice > 0) tradeResult.ExitPrice = Update.ExitPrice;
            if (Update.NumberOfStocks > 0) tradeResult.NumberOfStocks = Update.NumberOfStocks;
            if (Update.EntryDate > new DateTime(2025, 1, 1, 0, 0, 0)) tradeResult.EntryDate = Update.EntryDate;
            if (Update.ExitDate > new DateTime(2025, 1, 1, 0, 0, 0) && Update.ExitDate > tradeResult.EntryDate)
                tradeResult.ExitDate = Update.ExitDate;
            if (Update.TradeMethod != null) tradeResult.TradeMethod = Update.TradeMethod;

            await _tradeResultRepository.UpdateAsync(tradeResult);
            await _tradeResultRepository.SaveAsync();

            return Ok();
        }

        [Authorize]
        [HttpPatch("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid TradeResultId");
            }

            var auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

            if (auth0Id == null)
            {
                return BadRequest("Invalid UserId");
            }

            var userData = await _userRepository.GetByAuth0IdAsync(auth0Id);

            if (userData == null)
            {
                return NotFound("User not found");
            }

            var tradeResult = await _dbContext.TradeResults.FindAsync(id);

            if (tradeResult == null)
            {
                return NotFound("TradeResult not found");
            }

            if (tradeResult.UserId != userData.UserId)
            {
                return BadRequest("Only the creator can delete the post");
            }

            await _tradeResultRepository.DeleteAsync(tradeResult);
            await _tradeResultRepository.SaveAsync();

            return Ok();
        }

        private bool IsInvalidTradeResult(TradeResult newTradeResult)
        {
            if (string.IsNullOrEmpty(newTradeResult.Symbol)) return true;
            if (newTradeResult.EntryPrice < 0 || newTradeResult.ExitPrice < 0) return true;
            if (newTradeResult.NumberOfStocks < 0) return true;
            if (newTradeResult.EntryDate < new DateTime(2025, 1, 1)) return true;
            if (newTradeResult.ExitDate < new DateTime(2025, 1, 1)) return true;
            if (newTradeResult.ExitDate < newTradeResult.EntryDate) return true;

            return false;
        }
    }
}
