using Fomo.Application.DTO.IndicatorsDTO;
using Fomo.Application.DTO.StockDataDTO;

namespace Fomo.Application.Services
{
    public interface IIndicatorService
    {
        List<decimal> GetSMA(List<ValuesDTO> values, int period);

        BollingerBandsDTO GetBollingerBands(List<ValuesDTO> values, int period, int k);

        StochasticDTO GetStochastic(List<ValuesDTO> values, int period, int smaperiod);

        List<decimal> GetRSI(List<ValuesDTO> values, int period);

        List<decimal> GetSmoothedRSI(List <ValuesDTO> values, int period);
    }
}
