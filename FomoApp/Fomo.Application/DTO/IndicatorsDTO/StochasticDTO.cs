namespace Fomo.Application.DTO.IndicatorsDTO
{
    public class StochasticDTO
    {
        public List<decimal> k { get; set; } = new List<decimal>();

        public List<decimal> d { get; set; } = new List<decimal>();
    }
}
