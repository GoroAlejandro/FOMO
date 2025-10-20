using Fomo.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace Fomo.Api.Controllers
{
    [Route("api/[controller]")]
    public class StocksController : Controller
    {
        private readonly TwelveDataService _twelveDataService;

        public StocksController (TwelveDataService twelveDataService)
        {
            _twelveDataService = twelveDataService;
        }

        [HttpGet]
        [ProducesResponseType<JsonResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _twelveDataService.getStocks();
            return Json(stocks);
        }

        [Route("timeseries/{symbol}")]
        [ProducesResponseType<JsonResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockTimeSeries(string symbol)
        {
            var timeseries = await _twelveDataService.getTimeSeries(symbol);
            return Json(timeseries);
        }
    }
}
