namespace Fomo.Application.DTO.TradeResult
{
    public class TradeResultCreateDTO
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public int NumberOfStocks { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public TradeMethodDTO? TradeMethod { get; set; }
    }
}
