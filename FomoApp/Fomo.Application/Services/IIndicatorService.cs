using Fomo.Application.DTO;

namespace Fomo.Application.Services
{
    public interface IIndicatorService
    {
        List<decimal> GetSMA(List<ValuesDTO> values, int period);

        BollingerBandsDTO GetBollingerBands(List<ValuesDTO> values, int period, int k);

        StochasticDTO GetStochastic(List<ValuesDTO> values, int period, int smaperiod);
    }
}
