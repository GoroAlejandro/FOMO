using Fomo.Application.DTO;
using Fomo.Application.Services;
using Fomo.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace Fomo.Api.Controllers
{
    [Route("api/[controller]")]
    public class StocksController : Controller
    {
        private readonly ITwelveDataService _twelveDataService;

        private readonly IIndicatorService _indicatorService;

        public StocksController (ITwelveDataService twelveDataService, IIndicatorService indicatorService)
        {
            _twelveDataService = twelveDataService;
            _indicatorService = indicatorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(StockResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _twelveDataService.GetStocks();
            return Ok(stocks);
        }

        [Route("timeseries/{symbol}")]
        [ProducesResponseType(typeof(ValuesResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockTimeSeries(string symbol)
        {
            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
            {
                return NotFound($"No data was found for the symbol {symbol}.");
            }

            return Ok(timeseries);
        }

        [Route("timeseries/{symbol}/sma/{period:int}")]
        [ProducesResponseType(typeof(List<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetStockSMA (string symbol, int period)
        {
            if (period < 1)
            {
                return BadRequest("Period must be greater than zero.");
            }

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (period > timeseries.Values.Count)
            {
                return BadRequest("Period cannot exceed the number of elements.");
            }

            var sma = _indicatorService.GetSMA(timeseries.Values, period);
            return Ok(sma);
        }

        [Route("timeseries/{symbol}/bollinger/{period:int}/{k:int}")]
        [ProducesResponseType(typeof(BollingerBandsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockBollingerBands(string symbol, int period, int k)
        {
            if (period < 1 || k < 1 || k > 5)
            {
                return BadRequest("Period and K must be greater than zero, and K must not exceed 5.");
            }

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (period > timeseries.Values.Count)
            {
                return BadRequest("Period cannot exceed the number of elements.");
            }

            var bollingerBands = _indicatorService.GetBollingerBands(timeseries.Values, period, k);
            return Ok(bollingerBands);
        }

        [Route("timeseries/{symbol}/stochastic/{period:int}/{smaperiod:int}")]
        [ProducesResponseType(typeof(BollingerBandsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStochasticOscillator(string symbol, int period, int smaperiod)
        {
            if (period < 1 ||  smaperiod < 1)
            {
                return BadRequest("Period and smaperiod must be greater than zero");
            }

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (period > timeseries.Values.Count)
            {
                return BadRequest("Period cannot exceed the number of elements.");
            }

            var stochasticOscillator = _indicatorService.GetStochastic(timeseries.Values, period, smaperiod);
            return Ok(stochasticOscillator);
        }
    }
}
