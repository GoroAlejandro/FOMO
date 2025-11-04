using Fomo.Application.DTO;

namespace Fomo.Api.Helpers
{
    public interface ITradeResultValidateHelper
    {
        bool IsValidTradeResultDTO(TradeResultDTO tradeResult);
    }
}
