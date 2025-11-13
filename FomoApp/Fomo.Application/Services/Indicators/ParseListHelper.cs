using Fomo.Application.DTO.StockDataDTO;
using System.Globalization;

namespace Fomo.Application.Services.Indicators
{
    public class ParseListHelper
    {
        public List<decimal> ParseList (List<ValuesDTO> values, Func<ValuesDTO, string> property)
        {
            var valuesStr = values
                .Select(property)
                .ToList();

            var valuesd = new List<decimal>();

            foreach (string value in valuesStr)
            {
                decimal Price = 0;
                decimal.TryParse(value, CultureInfo.InvariantCulture, out Price);
                valuesd.Add(Price);
            }

            valuesd.Reverse();

            return valuesd;
        }

    }
}
