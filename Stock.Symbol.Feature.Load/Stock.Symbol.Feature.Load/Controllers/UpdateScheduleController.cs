using Microsoft.AspNetCore.Mvc;
using Stock.Symbol.Feature.Load.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateScheduleController : Controller
    {

        private readonly UpdateScheduleManager _updateScheduleManager;

        public UpdateScheduleController(UpdateScheduleManager updateScheduleManager)
        {
            _updateScheduleManager = updateScheduleManager;
        }

        [HttpGet]
        // GET: AccessKeyController
        public async Task<ActionResult<IList<DateTime>>> GetUpdateSchedule()
        {
            
            
            var updateDate = await _updateScheduleManager.GetUpdateScheduleAsync();

            if (updateDate == DateTime.MaxValue)
            {

                return StatusCode(500);
            }

            return Ok(new List<DateTime>() { updateDate });
        }
    }
}
