using Fomo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services.Indicators
{
    public class SmaCalculator
    {
        public List<decimal> CalculateSMA(List<ValuesDTO> values, int period)
        {
            var parser = new ParseListHelper();
            var valuesd = parser.ParseList(values);

            var sma = new List<decimal>();

            if (valuesd == null || valuesd.Count < period)
            {
                return sma;
            }

            decimal sum = valuesd.Take(period).Sum();
            sma.Add(sum / period);

            for (int i = period; i < valuesd.Count; i++)
            {
                sum = sum + valuesd[i] - valuesd[i - period];
                sma.Add(sum / period);
            }

            return sma;
        }
    }
}
    
