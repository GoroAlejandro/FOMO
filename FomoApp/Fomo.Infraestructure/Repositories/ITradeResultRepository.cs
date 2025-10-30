using Fomo.Domain.Entities;

namespace Fomo.Infrastructure.Repositories
{
    public interface ITradeResultRepository
    {
        Task<List<TradeResult>> GetAllAsync();
        Task<List<TradeResult>> GetUserResultsAsync(int userId);
        Task InsertAsync(TradeResult tradeResult);
        Task UpdateAsync(TradeResult tradeResult);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
