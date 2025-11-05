using Fomo.Application.DTO.IndicatorsDTO;
using Fomo.Application.DTO.StockDataDTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services.Indicators
{
    public class StochasticCalculator
    {
        public StochasticDTO CalculateStochastic(List<ValuesDTO> values, int period, int smaperiod) 
        {
            if (values == null || values.Count < period || values.Count - period +1 < smaperiod)
            {
                return new StochasticDTO
                {
                    k = new List<decimal>(),
                    d = new List<decimal>()
                };
            }

            var parser = new ParseListHelper();
            var lowList = parser.ParseList(values, v => v.Low);
            var highList = parser.ParseList(values, v => v.High);
            var close = parser.ParseList(values, v => v.Close);

            var calculator = new SmaCalculator();            

            var stochasticK = new List<decimal>();
            var stochasticD = new List<decimal>();

            for (int i = period - 1; i < values.Count; i++)
            {
                var lows = lowList.GetRange(i - period + 1 , period);
                var highs = highList.GetRange(i - period + 1, period);

                decimal min = lows.Min();
                decimal max = highs.Max();

                decimal c = close[i];

                decimal divisor = max - min;

                decimal kPoint = divisor == 0 ? 0 : ((c - min)/divisor)*100;
                stochasticK.Add(kPoint);
            }

            stochasticD = calculator.CalculateSMA(stochasticK, smaperiod);

            return new StochasticDTO 
            {
                k = stochasticK,
                d = stochasticD,
            };
        }
    }
}
