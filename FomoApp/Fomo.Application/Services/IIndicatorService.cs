using Fomo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.Services
{
    public interface IIndicatorService
    {
        List<decimal> GetSMA(List<ValuesDTO> values, int period);

        BollingerBandsDTO GetBollingerBands(List<ValuesDTO> values, int period, int k);
    }
}
