using Fomo.Application.DTO;

namespace Fomo.Infraestructure
{
    public interface ITwelveDataService
    {
        Task<StockResponseDTO> GetStocks();

        Task<ValuesResponseDTO> GetTimeSeries(string symbol);

    }
}
