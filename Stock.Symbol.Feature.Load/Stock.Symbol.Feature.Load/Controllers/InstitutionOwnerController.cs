using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class InstitutionOwnerController : Controller
    {
        private readonly InstitutionOwnerManager _institutionOwnerManager;

        public InstitutionOwnerController(InstitutionOwnerManager institutionOwnerManager)
        {
            _institutionOwnerManager = institutionOwnerManager;
        }

        // GET: 
        [HttpGet]
        [Route("institutions")]
        public async Task<ActionResult<List<InstitutionOwner>>> GetInstitutionOwnersFromRapid(string symbol)
        {
            var owners = await _institutionOwnerManager.GetInstitutionOwnersFromRapidAsync(symbol);


            if (owners == null || !owners.Any())
            { 
                owners = await _institutionOwnerManager.GetInstitutionOwnersFromHoldingsChannelAsync(symbol);
            }

            if (owners == null || !owners.Any())
            {
                var transaction = await _institutionOwnerManager.GetInstitutionOwnersTransactionFromHTMLFintelAsync(symbol);
                owners = await _institutionOwnerManager.GetInstitutionOwnersFromTransaction(transaction);
            }
                

            if (owners == null)
            {
                return StatusCode(500);
            }

            return Ok(owners);
        }   

        [HttpGet]
        [Route("institutions/fintel")]
        public async Task<ActionResult<List<InstitutionOwner>>> GetInstitutionOwnersFromFintel(string symbol)
        {
            var transaction = await _institutionOwnerManager.GetInstitutionOwnersTransactionFromHTMLFintelAsync(symbol);
            var owners = await _institutionOwnerManager.GetInstitutionOwnersFromTransaction(transaction);

            if (owners == null)
            {
                return StatusCode(500);
            }

            return Ok(owners);
        }   


        [HttpGet]
        [Route("institutions/holdingchannel")]
        public async Task<ActionResult<List<InstitutionOwner>>> GetInstitutionOwnersFromHoldingsChannel(string symbol)
        {
            var owners = await _institutionOwnerManager.GetInstitutionOwnersFromHoldingsChannelAsync(symbol);
            
            if (owners == null)
            {
                return StatusCode(500);
            }

            return Ok(owners);
        }   


        [HttpGet]
        [Route("institutions/fintel/transaction")]
        public async Task<ActionResult<List<InstitutionTransaction>>> GetInstitutionTransaction(string symbol)
        {
            var trans = await _institutionOwnerManager.GetInstitutionOwnersTransactionFromHTMLFintelAsync(symbol);

            if (trans == null)
            {
                return StatusCode(500);
            }

            return Ok(trans);
        }   
    }
}
