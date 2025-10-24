namespace Fomo.Application.Services.Indicators
{
    public class SmaCalculator
    {
        public List<decimal> CalculateSMA(List<decimal> values, int period)
        {

            var sma = new List<decimal>();

            if (values == null || values.Count < period)
            {
                return sma;
            }

            decimal sum = values.Take(period).Sum();
            sma.Add(sum / period);

            for (int i = period; i < values.Count; i++)
            {
                sum = sum + values[i] - values[i - period];
                sma.Add(sum / period);
            }

            return sma;
        }
    }
}
    
