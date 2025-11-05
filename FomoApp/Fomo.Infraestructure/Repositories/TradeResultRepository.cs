using Fomo.Application.DTO;
using Fomo.Application.DTO.TradeResult;
using Fomo.Domain.Entities;
using Fomo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fomo.Infrastructure.Repositories
{
    public class TradeResultRepository : ITradeResultRepository
    {
        private readonly EFCoreDbContext _dbContext;

        public TradeResultRepository(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TradeResult?> GetByIdAsync(int id)
        {
            var tradeResult = await _dbContext.TradeResults
                .Select(tr => new TradeResult
                {
                    TradeResultId = tr.TradeResultId,
                    Symbol = tr.Symbol,
                    EntryPrice = tr.EntryPrice,
                    ExitPrice = tr.ExitPrice,
                    NumberOfStocks = tr.NumberOfStocks,
                    EntryDate = tr.EntryDate,
                    ExitDate = tr.ExitDate,
                    UserId = tr.UserId,
                    TradeMethod = new TradeMethod
                    {
                        Sma = tr.TradeMethod.Sma,
                        Bollinger = tr.TradeMethod.Bollinger,
                        Stochastic = tr.TradeMethod.Stochastic,
                        Rsi = tr.TradeMethod.Rsi,
                        Other = tr.TradeMethod.Other
                    }
                })
                .FirstOrDefaultAsync(tr => tr.TradeResultId == id);

            return tradeResult;
        }

        public async Task<List<TradeResultDTO>> GetAllAsync()
        {
            var resultsList = await _dbContext.TradeResults
                .Include(tr => tr.TradeMethod)
                .Include(tr => tr.User)
                .Select(tr => new TradeResultDTO
                {
                    TradeResultId = tr.TradeResultId,
                    Symbol = tr.Symbol,
                    EntryPrice = tr.EntryPrice,
                    ExitPrice = tr.ExitPrice,
                    Profit = tr.Profit,
                    NumberOfStocks = tr.NumberOfStocks,
                    EntryDate = tr.EntryDate,
                    ExitDate = tr.ExitDate,
                    TradeMethod = new TradeMethodDTO
                    {
                        Sma = tr.TradeMethod.Sma,
                        Bollinger = tr.TradeMethod.Bollinger,
                        Stochastic = tr.TradeMethod.Stochastic,
                        Rsi = tr.TradeMethod.Rsi,
                        Other = tr.TradeMethod.Other
                    },
                    UserName = tr.User.Name                    
                })
                .AsNoTracking()
                .ToListAsync();

            return resultsList;
        }

        public async Task InsertAsync(TradeResult tradeResult)
        {
            await _dbContext.TradeResults.AddAsync(tradeResult);            
        }

        public async Task UpdateAsync(TradeResult tradeResult)
        {
            _dbContext.TradeResults.Update(tradeResult);
        }

        public async Task DeleteAsync(TradeResult tradeResult)
        {            
            if (tradeResult != null)
            {
                _dbContext.TradeResults.Remove(tradeResult);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
