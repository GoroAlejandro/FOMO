using Fomo.Application.DTO;

namespace Fomo.Application.Services.Indicators
{
    public class BollingerCalculator
    {
        public BollingerBandsDTO CalculateBollinger(List<ValuesDTO> values, int period, int k)
        {
            if (values == null || values.Count < period || period == 0)
            {
                return new BollingerBandsDTO
                {
                    UpperBand = new List<decimal>(),
                    LowerBand = new List<decimal>()
                };
            }            

            var parser = new ParseListHelper();
            var valuesd = parser.ParseList(values, v => v.Close);

            var calculator = new SmaCalculator();
            var sma = calculator.CalculateSMA(valuesd, period);

            var bollingerUpper = new List<decimal>();
            var bollingerLower = new List<decimal>();

            for (int i = period - 1; i < valuesd.Count; i++)
            {
                var subList = valuesd.GetRange(i - period + 1 , period);

                decimal variance = 0;

                var smaValue = sma[i - (period - 1)];

                foreach (var value in subList)
                {
                    variance = variance + ((value - smaValue) * (value - smaValue));
                }

                variance = variance / period;
                var stdDev = (decimal)Math.Sqrt((double)variance);

                bollingerUpper.Add(smaValue + k * stdDev);
                bollingerLower.Add(smaValue - k * stdDev);
            }

            return new BollingerBandsDTO
            {
                UpperBand = bollingerUpper,
                LowerBand = bollingerLower,
            };
        }
    }
}

