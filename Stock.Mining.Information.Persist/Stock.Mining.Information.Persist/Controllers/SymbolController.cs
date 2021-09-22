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
    public class SymbolController : Controller
    {
        
        private readonly SymbolManager _symbolManager;

        public SymbolController(SymbolManager symbolManager)
        {
            _symbolManager = symbolManager;
        }
        
        [HttpGet("symbols")]
        public async Task<ActionResult<List<Symbol>>> GetSymbols()
        {
            var symbols = await _symbolManager.GetAllSymbols();

            if (symbols == null || !symbols.Any())
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(symbols);
            
        }

        [HttpGet("symbol")]
        public async Task<ActionResult<Symbol>> GetSymbol(string tickerName)
        {
            var symbol = await _symbolManager.GetSymbolByName(tickerName);

            if (symbol == null)
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(symbol);
        
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<Symbol>> UpsertSymbol(Symbol ticker)
        {
            var result = await _symbolManager.AddOrUpdateSymbol(ticker);

            if (result == null)
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(result);
        
        }

        [HttpPost("disablesymbolscan")]
        public async Task<ActionResult<Symbol>> DisableSymbolScan(string tickerName)
        {
             var result = await _symbolManager.DisableSymbolScanning(tickerName);

             if (result == false)
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(result);   
        }
    }
}
