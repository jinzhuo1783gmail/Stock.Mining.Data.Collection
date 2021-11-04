using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.EF.Core
{
    public class UpdateScheduleRepository
    {
        public RSDBContext _context;
        public ILogger<UpdateScheduleRepository> _logger;

        public UpdateScheduleRepository(RSDBContext context, ILogger<UpdateScheduleRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public UpdateSchedule Get()
        {
            return _context.UpdateSchedule.FirstOrDefault();
        }

    }
}
