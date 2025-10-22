﻿using Fomo.Application.DTO;
using Fomo.Infraestructure.ExternalServices;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Fomo.Infraestructure
{
    public class TwelveDataService : ITwelveDataService
    {
        private readonly IExternalApiHelper _externalApiHelper;
        private readonly TwelveData _twelveData;

        public TwelveDataService(IOptions<TwelveData> options, IExternalApiHelper externalApiHelper)
        {
            _externalApiHelper = externalApiHelper;
            _twelveData = options.Value;
        }

        public async Task<StockResponseDTO> GetStocks()
        {
            string path = $"stocks?country=US&apikey={_twelveData.ApiKey}";
            
            return await _externalApiHelper.GetAsync<StockResponseDTO>(path);
        }

        public async Task<ValuesResponseDTO> GetTimeSeries(string symbol)
        {
            string path = $"time_series?symbol={symbol}&interval=1day&outputsize=120&apikey={_twelveData.ApiKey}";

            return await _externalApiHelper.GetAsync<ValuesResponseDTO>(path);
        }
    }
}
