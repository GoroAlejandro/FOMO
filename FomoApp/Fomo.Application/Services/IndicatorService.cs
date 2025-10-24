using Fomo.Application.DTO;
using Fomo.Application.Services.Indicators;

namespace Fomo.Application.Services
{
    public class IndicatorService : IIndicatorService
    {

        public List<decimal> GetSMA (List<ValuesDTO> values, int period)
        {
            var parser = new ParseListHelper();
            var valuesd = parser.ParseList(values, v => v.Close);
            var calculator = new SmaCalculator();
            return calculator.CalculateSMA(valuesd, period);
        }

        public BollingerBandsDTO GetBollingerBands (List<ValuesDTO> values, int period, int k)
        {
            var calculator = new BollingerCalculator();
            return calculator.CalculateBollinger(values, period, k);
        }

        public StochasticDTO GetStochastic (List<ValuesDTO> values, int period, int smaperiod)
        {
            var calculator = new StochasticCalculator();
            return calculator.CalculateStochastic(values, period, smaperiod);
        }
    }
}
