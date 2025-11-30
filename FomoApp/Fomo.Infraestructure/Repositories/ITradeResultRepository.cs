using Fomo.Application.DTO.TradeResult;
using Fomo.Domain.Entities;

namespace Fomo.Infrastructure.Repositories
{
    public interface ITradeResultRepository
    {
        Task<TradeResult?> GetByIdAsync(int id);
        Task<List<TradeResultDTO>> GetAllAsync();
        Task InsertAsync(TradeResult tradeResult);
        void UpdateAsync(TradeResult tradeResult);
        void DeleteAsync(TradeResult tradeResult);
        Task SaveAsync();
    }
}
