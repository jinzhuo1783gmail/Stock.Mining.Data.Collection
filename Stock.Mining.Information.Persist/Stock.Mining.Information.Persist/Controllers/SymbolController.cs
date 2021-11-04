using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Collection.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Mining.Information.Persist.Manager;
using Newtonsoft.Json;

namespace Stock.Mining.Information.Collection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymbolController : Controller
    {
        
        private readonly SymbolManager _symbolManager;
        private readonly SymbolUpdateHistoryManager _symbolUpdateHistoryManager;

        public SymbolController(SymbolManager symbolManager, SymbolUpdateHistoryManager symbolUpdateHistoryManager)
        {
            _symbolManager = symbolManager;
            _symbolUpdateHistoryManager = symbolUpdateHistoryManager;
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

        [HttpGet("symbol/updatehistories")]
        public async Task<ActionResult<List<SymbolUpdateHistory>>> GetUpdateHistories(string tickerName)
        {
            var hist = await _symbolUpdateHistoryManager.GetAllUpdates(tickerName);

            if (hist == null || !hist.Any())
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(hist);
            
        }

        [HttpGet("symbol/nextupdatetime")]
        public async Task<ActionResult<DateTime>> GetNextUpdateTime(string tickerName)
        {
            var lastUpdate = await _symbolUpdateHistoryManager.GetLastUpdate(tickerName);

            if (lastUpdate == null)
            { 
                return DateTime.MinValue; 
            }

            return Ok(lastUpdate.NextUpdateTime);
            
        }

        [HttpPost("symbol/updatehistories/insert")]
        public async Task<ActionResult<bool>> InsertUpdateHistory(IList<SymbolUpdateHistory> histories)
        {
            if ( histories.Any(h => h.Id != default(long)))
                return BadRequest($"update history id is not 0 for insert {JsonConvert.SerializeObject(histories)}");

            var result = await _symbolUpdateHistoryManager.AddUpdateHistories(histories);;

            if (result == false)
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(result);   
        }


        [HttpPost("symbol/updatehistories/update")]
        public async Task<ActionResult<bool>> UpdateUpdateHistory(IList<SymbolUpdateHistory> histories)
        {
            if ( histories.Any(h => h.Id == default(long)))
                return BadRequest($"update history id is 0 for update {JsonConvert.SerializeObject(histories)}");
            

            var result = await _symbolUpdateHistoryManager.UpdateUpdateHistories(histories);

             if (result == false)
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }

            return Ok(result);   
        }

    }
}
