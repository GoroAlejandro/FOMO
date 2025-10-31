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
            return await _dbContext.TradeResults.Include(tr => tr.TradeMethod).Include(tr => tr.User).ToListAsync();
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
