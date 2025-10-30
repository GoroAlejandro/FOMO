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

        public async Task<List<TradeResult>> GetAllAsync()
        {
            return await _dbContext.TradeResults.Include(tr => tr.TradeMethod).ToListAsync();
        }

        public async Task<List<TradeResult>> GetUserResultsAsync(int userId)
        {
            return await _dbContext.TradeResults.Include(tr => tr.TradeMethod).Where(tr => tr.UserId == userId).ToListAsync();
        }

        public async Task InsertAsync(TradeResult tradeResult)
        {
            await _dbContext.TradeResults.AddAsync(tradeResult);            
        }

        public async Task UpdateAsync(TradeResult tradeResult)
        {
            _dbContext.TradeResults.Update(tradeResult);
        }

        public async Task DeleteAsync(int id)
        {
            var tradeResult = await _dbContext.TradeResults.FindAsync(id);
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
