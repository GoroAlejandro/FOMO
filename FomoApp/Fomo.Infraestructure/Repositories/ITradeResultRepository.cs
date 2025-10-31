using Fomo.Domain.Entities;

namespace Fomo.Infrastructure.Repositories
{
    public interface ITradeResultRepository
    {
        Task<List<TradeResult>> GetAllAsync();
        Task InsertAsync(TradeResult tradeResult);
        Task UpdateAsync(TradeResult tradeResult);
        Task DeleteAsync(TradeResult tradeResult);
        Task SaveAsync();
    }
}
