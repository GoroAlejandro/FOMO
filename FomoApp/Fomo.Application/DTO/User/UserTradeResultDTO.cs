using Fomo.Application.DTO.TradeResult;

namespace Fomo.Application.DTO.User
{
    public class UserTradeResultDTO
    {
        public int TradeResultId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal Profit { get; set; }
        public int NumberOfStocks { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public TradeMethodDTO? TradeMethod { get; set; }
    }
}
