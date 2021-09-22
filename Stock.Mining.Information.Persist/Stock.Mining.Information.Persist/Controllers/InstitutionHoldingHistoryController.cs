using Microsoft.AspNetCore.Http;
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
    
    public class InstitutionHoldingHistoryController : Controller
    {
        private readonly InstitutionHoldingHistoryManager _institutionHoldingHistoryManager;

        public InstitutionHoldingHistoryController(InstitutionHoldingHistoryManager institutionHoldingHistoryManager)
        {
            _institutionHoldingHistoryManager = institutionHoldingHistoryManager;
        }

        [HttpGet]
        [Route("institutionhistories")]
        public async Task<ActionResult<List<InstitutionHoldingsHistory>>> GetInstitutionHoldingHistoryById(int Id)
        {
            
            var result = await _institutionHoldingHistoryManager.GetInstitutionHoldingsHistoriesByInstitutionIdAsync(Id);

            if (result == null || !result.Any())
            {
                return BadRequest($"Cannot find any history via institution Id {Id}");
            }
            
            return Ok(result);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<bool>> AddInstitutionHoldingHistories(IList<InstitutionHoldingsHistory> History)
        {
            var result = await _institutionHoldingHistoryManager.AddInstitutionHoldingHistories(History);

            if (!result)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }
            
            return Ok(result);
        }
    }
}
