using Fomo.Domain.Entities;
using Fomo.Infrastructure.ExternalServices.StockService;
using Fomo.Infrastructure.Repositories;

namespace Fomo.Api.Helpers
{
    public class StockSyncService : IStockSyncService
    {
        private readonly IStockRepository _stockRepository;
        private readonly ITwelveDataService _twelveDataService;

        public StockSyncService (IStockRepository stockRepository, ITwelveDataService twelveDataService)
        {
            _stockRepository = stockRepository;
            _twelveDataService = twelveDataService;
        }

        public async Task SyncStockDb()
        {
            var stocksDTO = await _twelveDataService.GetStocks();

            if (stocksDTO?.Data == null || stocksDTO.Data.Count == 0)
                return;

            var numberOfRecords = await _stockRepository.CountRecordsAsync();

            if (stocksDTO.Data.Count != numberOfRecords)
            {
                using var transaction = await _stockRepository.BeginTransactionAsync();
                try
                {
                    var stocks = stocksDTO.Data
                        .Select(dto => new Stock
                        {
                            Symbol = dto.Symbol,
                            Name = dto.Name,
                            Currency = dto.Currency,
                            Exchange = dto.Exchange
                        })
                        .ToList();

                    await _stockRepository.TruncateAsync();
                    await _stockRepository.InsertListAsync(stocks);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

    }
}
