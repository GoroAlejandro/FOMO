using Fomo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services.Indicators
{
    public class BollingerCalculator
    {
        public BollingerBandsDTO CalculateBollinger(List<ValuesDTO> values, int period, int k)
        {
            var calculator = new SmaCalculator();
            var sma = calculator.CalculateSMA(values, period);

            var parser = new ParseListHelper();
            var valuesd = parser.ParseList(values);

            var bollingerUpper = new List<decimal>();
            var bollingerLower = new List<decimal>();

            for (int i = period - 1; i < valuesd.Count; i++)
            {
                var subList = valuesd.Skip(i - period + 1).Take(period).ToList();

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

