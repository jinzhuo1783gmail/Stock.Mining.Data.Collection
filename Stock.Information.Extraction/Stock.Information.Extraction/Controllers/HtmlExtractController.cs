using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stock.Information.Extraction.Manager;
using Stock.Extraction.Library.Request;

namespace Stock.Information.Extraction.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HtmlExtractController : Controller
    {


        private HtmlExtractionManager _htmlExtractionManager;
        private readonly ILogger<HtmlExtractController> _logger;
        public HtmlExtractController(ILogger<HtmlExtractController> logger, HtmlExtractionManager htmlExtractionManager)
        {
            _logger = logger;
            _htmlExtractionManager = htmlExtractionManager;
        }

        [HttpGet]
        [Route("HealthCheck", Name = "HealthCheck")]
        
        public async Task<ActionResult<bool>> HealthCheck()
        {
            return Ok(true);
        }

        // POST: HtmlExtractController/Create
        [HttpPost]
        [Route("ExtractTable")]
        public async Task<ActionResult<List<DataTable>>> ExtractTable(HtmlTableRequest url)
        {
            try
            {
                var tables = await _htmlExtractionManager.ExtractTable(url.Url);
                return Ok(tables);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"[ExtractTable]: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("ExtractText")]
        public ActionResult ExtractText(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"[ExtractText]: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}
