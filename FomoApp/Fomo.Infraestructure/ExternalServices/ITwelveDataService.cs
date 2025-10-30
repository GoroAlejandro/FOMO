using Fomo.Application.DTO;

namespace Fomo.Infrastructure.ExternalServices
{
    public interface ITwelveDataService
    {
        Task<StockResponseDTO> GetStocks();

        Task<ValuesResponseDTO> GetTimeSeries(string symbol);

    }
}
