namespace Fomo.Domain.Entities
{
    public class TradeMethod
    {
        public int TradeMethodId { get; set; }
        public bool Sma { get; set; } = false;
        public bool Bollinger { get; set; } = false;
        public bool Stochastic { get; set; } = false;
        public bool Rsi { get; set; } = false;
        public bool Other { get; set; } = true;
        public int TradeResultId { get; set; }

        public TradeResult? TradeResult { get; set; }


    }
}
