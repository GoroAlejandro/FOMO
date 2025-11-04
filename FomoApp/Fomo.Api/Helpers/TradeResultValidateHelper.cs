using Fomo.Application.DTO;

namespace Fomo.Api.Helpers
{
    public class TradeResultValidateHelper : ITradeResultValidateHelper
    {
        public bool IsValidTradeResultDTO(TradeResultDTO tradeResult)
        {
            if (String.IsNullOrEmpty(tradeResult.Symbol) || tradeResult.EntryPrice <= 0 || tradeResult.ExitPrice <= 0 ||
                tradeResult.NumberOfStocks <= 0 || tradeResult.EntryDate < new DateTime(2025, 1, 1, 0, 0, 0) ||
                tradeResult.ExitDate < new DateTime(2025, 1, 1, 0, 0, 0) || tradeResult.ExitDate < tradeResult.EntryDate) 
            {
                return false;
            }

            return true;
        }
    }
}
