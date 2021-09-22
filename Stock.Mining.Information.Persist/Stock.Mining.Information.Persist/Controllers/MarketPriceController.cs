using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Persist.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Persist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MarketPriceController : Controller
    {
        private readonly SymbolPriceManager _symbolPriceManager;

        public MarketPriceController(SymbolPriceManager symbolPriceManager)
        {
            _symbolPriceManager = symbolPriceManager;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<SymbolPrice>>> GetMarketPriceTickerName(string TickerName, DateTime startDate, DateTime endDate)
        {

            startDate = startDate != DateTime.MinValue ? startDate : DateTime.MinValue;
            endDate = endDate != DateTime.MinValue ? endDate.Date.AddDays(1).AddSeconds(-1) : DateTime.MaxValue;


            var result = await _symbolPriceManager.GetAllPrice(TickerName, startDate, endDate);

            if (result == null || !result.Any())
            {
                return BadRequest($"Cannot find any InstitutionHoldings via ticker name {TickerName}");
            }
            
            return Ok(result);
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<bool>> AddOrUpdateInstitutionHoldings(IList<SymbolPrice> marketPrice)
        {
            
            var result = await _symbolPriceManager.AddOrUpdateBatchPrices(marketPrice);

            if (result == false)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
            return Ok(result);
        }
    }
}
