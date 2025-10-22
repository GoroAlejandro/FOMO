using Fomo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services.Indicators
{
    public class ParseListHelper
    {
        public List<decimal> ParseList (List<ValuesDTO> values)
        {
            var valuesd = new List<decimal>();

            foreach (ValuesDTO value in values)
            {
                decimal closePrice = 0;
                decimal.TryParse(value.Close, CultureInfo.InvariantCulture, out closePrice);
                valuesd.Add(closePrice);
            }

            return valuesd;
        }
    }
}
