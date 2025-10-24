namespace Fomo.Application.DTO
{
    public class BollingerBandsDTO
    {
        public List<decimal> UpperBand {  get; set; } = new List<decimal>();

        public List<decimal> LowerBand { get; set; } = new List<decimal>();
    }
}
