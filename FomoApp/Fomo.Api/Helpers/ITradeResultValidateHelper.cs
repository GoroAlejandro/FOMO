using Fomo.Application.DTO.TradeResult;

namespace Fomo.Api.Helpers
{
    public interface ITradeResultValidateHelper
    {
        bool IsValidTradeResultDTO(TradeResultCreateDTO tradeResult);
    }
}
