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
    public class InsiderTransactionController : Controller
    {
        
        private readonly InsiderHistoryManager _insiderHistoryManager;

        public InsiderTransactionController(InsiderHistoryManager _insiderHistoryManager)
        {
            _insiderHistoryManager = _insiderHistoryManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<InsiderHistory>>> GetInsiderTransactionTickerName(string TickerName)
        {

            var result = await _insiderHistoryManager.GetAllHsitories(TickerName);

            if (result == null || !result.Any())
            {
                return BadRequest($"Cannot find any InsiderTransaction via ticker name {TickerName}");
            }
            
            return Ok(result);
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<bool>> AddOrUpdateInsiderHistories(IList<InsiderHistory> histories)
        {
            
            var result = await _insiderHistoryManager.AddOrUpdateBatchHsitories(histories);

            if (result == false)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
            return Ok(result);
        }
    }
}
