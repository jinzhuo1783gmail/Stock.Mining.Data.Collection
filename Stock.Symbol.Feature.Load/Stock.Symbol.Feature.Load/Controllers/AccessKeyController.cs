using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.Symbol.Feature.EF.Core;
using Stock.Symbol.Feature.Load.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessKeyController : Controller
    {

        private readonly RapidApiManager _rapidApiManager;
        private ILogger<AccessKeyController> _logger;
        public AccessKeyController(RapidApiManager rapidApiManager, ILogger<AccessKeyController> logger)
        {
            _rapidApiManager = rapidApiManager;
            _logger = logger;
        }

        [HttpGet]
        // GET: AccessKeyController
        public async Task<ActionResult<List<RestSharpAccessKey>>> GetAccessKeys()
        {
            var keys = await _rapidApiManager.GetAllKeysAsync();

            if (keys == null)
            {
                _logger.LogError("No key returned from db");
                return StatusCode(500);
            }

            return Ok(keys);
        }
        
    }
}
