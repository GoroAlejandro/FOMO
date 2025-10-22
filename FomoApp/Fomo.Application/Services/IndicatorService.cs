using Fomo.Application.DTO;
using Fomo.Application.Services.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services
{
    public class IndicatorService : IIndicatorService
    {

        public List<decimal> GetSMA (List<ValuesDTO> values, int period)
        {
            var calculator = new SmaCalculator();
            return calculator.CalculateSMA(values, period);
        }

        public BollingerBandsDTO GetBollingerBands (List<ValuesDTO> values, int period, int k)
        {
            var calculator = new BollingerCalculator();
            return calculator.CalculateBollinger(values, period, k);
        }
    }
}
