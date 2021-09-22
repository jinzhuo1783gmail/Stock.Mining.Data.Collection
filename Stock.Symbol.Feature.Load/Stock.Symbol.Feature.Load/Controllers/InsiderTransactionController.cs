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
    public class InsiderTransactionController : Controller
    {
        private readonly InsiderTransactionManager _insiderTransactionManager;

        private ILogger<InsiderTransactionController> _logger;
        public InsiderTransactionController(InsiderTransactionManager insiderTransactionManager, ILogger<InsiderTransactionController> logger)
        {
            _insiderTransactionManager = insiderTransactionManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("rapid/insidertransaction")]
        public async Task<ActionResult<List<InsiderTransaction>>> GetInsiderFromRapid(string symbol)
        {
            var transList = await _insiderTransactionManager.GetInsiderTransaction(symbol);

            if (transList == null || !transList.Any())
            {
                return StatusCode(500);
            }

            return Ok(transList);
        }   
    }
}
