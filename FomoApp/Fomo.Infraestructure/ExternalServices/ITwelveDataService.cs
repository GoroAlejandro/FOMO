using Fomo.Application.DTO.StockDataDTO;

namespace Fomo.Infrastructure.ExternalServices
{
    public interface ITwelveDataService
    {
        Task<StockResponseDTO> GetStocks();

        Task<ValuesResponseDTO> GetTimeSeries(string symbol);

    }
}
