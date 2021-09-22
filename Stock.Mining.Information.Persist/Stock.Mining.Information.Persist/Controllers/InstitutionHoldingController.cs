using Microsoft.AspNetCore.Mvc;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Collection.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Collection.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionHoldingController : Controller
    {

        private readonly InstitutionHoldingManager _institutionHoldingManager;

        public InstitutionHoldingController(InstitutionHoldingManager institutionHoldingManager)
        {
            _institutionHoldingManager = institutionHoldingManager;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<InstitutionHolding>>> GetInstitutionHoldingByTickerName(string TickerName, bool IncludeAllSymbolAndHistory = false)
        {
            
            var result = await _institutionHoldingManager.GetInstitutionHoldings(TickerName, IncludeAllSymbolAndHistory);

            if (result == null || !result.Any())
            {
                return BadRequest($"Cannot find any InstitutionHoldings via ticker name {TickerName}");
            }
            
            return Ok(result);
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<List<InstitutionHolding>>> AddAndUpdateInstitutionHoldings(IList<InstitutionHolding> InstitutionHoldings)
        {
            
            var result = await _institutionHoldingManager.AddOrUpdateInstitutionHoldings(InstitutionHoldings);

            if (result == null)
            {
                return BadRequest($"failed to upsert InstitutionHoldings");
            }
            
            return Ok(result);
        }

    }
}
