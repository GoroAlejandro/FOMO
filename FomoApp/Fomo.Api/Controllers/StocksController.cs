using Fomo.Application.DTO.IndicatorsDTO;
using Fomo.Application.DTO.StockDataDTO;
using Fomo.Application.Services;
using Fomo.Application.Services.Indicators;
using Fomo.Domain.Entities;
using Fomo.Infrastructure.ExternalServices.MailService;
using Fomo.Infrastructure.ExternalServices.StockService;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fomo.Api.Controllers
{
    [Route("api/[controller]")]
    public class StocksController : Controller
    {
        private readonly ITwelveDataService _twelveDataService;
        private readonly IIndicatorService _indicatorService;
        private readonly IAlertService _alertService;
        private readonly IStockRepository _stockRepository;

        public StocksController(ITwelveDataService twelveDataService, IIndicatorService indicatorService,
            IAlertService alertService, IStockRepository stockRepository)
        {
            _twelveDataService = twelveDataService;
            _indicatorService = indicatorService;
            _alertService = alertService;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SymbolAndName), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockRepository.GetStocks();

            if (stocks == null)
                return NotFound("Cannot obtain StockData");

            return Ok(stocks);
        }

        [HttpGet("find/{query}")]
        [ProducesResponseType(typeof(SymbolAndName), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStocksFiltered(string query)
        {
            var stocks = await _stockRepository.GetFilteredStocks(query);

            if (stocks == null)
                return NotFound("Cannot obtain StockData");

            return Ok(stocks);
        }

        [HttpGet("{page:int}/{pagesize:int}")]
        [ProducesResponseType(typeof(StockResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStocksPage(int page, int pagesize)
        {
            if (page <= 0 || pagesize <= 0)
                return BadRequest("Page and PageSize must be greatear than 0");

            var totalRecords = await _stockRepository.CountRecordsAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords/pagesize);

            if (page > totalPages)
                return Ok(new StockPageDTO
                {
                    Data = [],
                    CurrentPage = page,
                    TotalPages = totalPages
                });

            var stocks = await _stockRepository.GetPaginatedStocks(page, pagesize);

            if (stocks == null)
                return NotFound("Cannot obtain StockData");

            return Ok(new StockPageDTO
            {
                Data = stocks,
                CurrentPage = page,
                TotalPages = totalPages
            });
        }

        [HttpGet("timeseries/{symbol}")]
        [ProducesResponseType(typeof(ValuesResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockTimeSeries(string symbol)
        {
            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            return Ok(timeseries);
        }

        [HttpGet("timeseries/{symbol}/sma/{period:int}")]
        [ProducesResponseType(typeof(List<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetStockSMA (string symbol, int period)
        {
            if (period < 1)
                return BadRequest("Period must be greater than zero.");

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            if (period > timeseries.Values.Count)
                return BadRequest("Period cannot exceed the number of elements.");

            var sma = _indicatorService.GetSMA(timeseries.Values, period);

            var parser = new ParseListHelper();
            var closes = parser.ParseList(timeseries.Values, v => v.Close);

            string indicator = $"SMA with a period of {period}";
            await _alertService.SendSmaAlert(closes, sma, symbol, indicator);

            return Ok(sma);
        }

        [HttpGet("timeseries/{symbol}/bollinger/{period:int}/{k:int}")]
        [ProducesResponseType(typeof(BollingerBandsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockBollingerBands(string symbol, int period, int k)
        {
            if (period < 1 || k < 1 || k > 5)
                return BadRequest("Period and K must be greater than zero, and K must not exceed 5.");

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            if (period > timeseries.Values.Count)
                return BadRequest("Period cannot exceed the number of elements.");

            var bollingerBands = _indicatorService.GetBollingerBands(timeseries.Values, period, k);

            var parser = new ParseListHelper();
            var closes = parser.ParseList(timeseries.Values, v => v.Close);

            string indicator = $"Bollinger Bands with a period of {period} and a k of {k}";
            await _alertService.SendBollingerAlert(closes, bollingerBands.LowerBand, symbol, indicator);

            return Ok(bollingerBands);
        }

        [HttpGet("timeseries/{symbol}/stochastic/{period:int}/{smaperiod:int}")]
        [ProducesResponseType(typeof(BollingerBandsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStochasticOscillator(string symbol, int period, int smaperiod)
        {
            if (period < 1 ||  smaperiod < 1)
                return BadRequest("Period and smaperiod must be greater than zero");

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            if (period > timeseries.Values.Count)
                return BadRequest("Period cannot exceed the number of elements.");

            var stochasticOscillator = _indicatorService.GetStochastic(timeseries.Values, period, smaperiod);

            string indicator = $"Stochastic Oscilator with a period of {period} and a SMA period of {smaperiod}";
            await _alertService.SendStochasticAlert(stochasticOscillator.k, stochasticOscillator.d, symbol, indicator);

            return Ok(stochasticOscillator);
        }

        [HttpGet("timeseries/{symbol}/rsi/{period:int}")]
        [ProducesResponseType(typeof(List<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockRSI(string symbol, int period)
        {
            if (period < 9 || period > 25)
                return BadRequest("Period must be between 9 and 25.");

            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            if (period > timeseries.Values.Count)
                return BadRequest("Period cannot exceed the number of elements.");

            var rsi = _indicatorService.GetRSI(timeseries.Values, period);

            string indicator = $"RSI with a period of {period}";
            await _alertService.SendRsiAlert(rsi, symbol, indicator);

            return Ok(rsi);
        }

        [HttpGet("timeseries/{symbol}/srsi/{period:int}")]
        [ProducesResponseType(typeof(List<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockSmoothedRSI(string symbol, int period)
        {
            if (period < 9 || period > 25)
                return BadRequest("Period must be between 9 and 25.");
            
            var timeseries = await _twelveDataService.GetTimeSeries(symbol);

            if (timeseries == null || timeseries.Values == null)
                return NotFound($"No data was found for the symbol {symbol}.");

            if (period > timeseries.Values.Count)
                return BadRequest("Period cannot exceed the number of elements."); 

            var srsi = _indicatorService.GetSmoothedRSI(timeseries.Values, period);

            string indicator = $"Smoothed RSI with a period of {period}";
            await _alertService.SendRsiAlert(srsi, symbol, indicator);

            return Ok(srsi);
        }
    }
}
