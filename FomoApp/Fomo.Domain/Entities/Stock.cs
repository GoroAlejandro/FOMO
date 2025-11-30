namespace Fomo.Domain.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public string Symbol { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Currency { get; init; } = string.Empty;
        public string Exchange { get; init; } = string.Empty;
    }
}
