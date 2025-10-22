using Fomo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Infraestructure
{
    public interface ITwelveDataService
    {
        Task<StockResponseDTO> GetStocks();

        Task<ValuesResponseDTO> GetTimeSeries(string symbol);

    }
}
