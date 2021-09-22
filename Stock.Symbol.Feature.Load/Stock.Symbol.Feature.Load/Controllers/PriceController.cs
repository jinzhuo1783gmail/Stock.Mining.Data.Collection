using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.Symbol.Feature.Load.Manager;
using Stock.Symbol.Feature.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class PriceController : Controller
    {

        private DailyPriceManager _dailyPriceManager;
        private ILogger<PriceController> _logger;
        public PriceController(DailyPriceManager dailyPriceManager, ILogger<PriceController> logger)
        {
            _dailyPriceManager = dailyPriceManager;
            _logger = logger;
        }

        // GET: 
        [HttpGet]
        [Route("daily")]
        public async Task<ActionResult<List<StockPrice>>> GetMarketPriceFromAlphaVantage(string symbol, DateTime startDate, DateTime endDate)
        {

            startDate = startDate == default(DateTime) ? DateTime.MinValue :  startDate.Date;
            endDate = endDate == default(DateTime) ? DateTime.MaxValue :  endDate.Date.AddDays(1).AddSeconds(-1);

            if (startDate > endDate)
            {
                _logger.LogError($"Start date is older than enddate! start: {startDate.ToString()} end: {endDate.ToString()}");
                return BadRequest();    
            }

            var prices = await _dailyPriceManager.GetStockDailyPrice(symbol, startDate, endDate);

            if (prices == null || !prices.Any())
            {
                return StatusCode(500);
            }

            return Ok(prices);
        }   

    }
}
