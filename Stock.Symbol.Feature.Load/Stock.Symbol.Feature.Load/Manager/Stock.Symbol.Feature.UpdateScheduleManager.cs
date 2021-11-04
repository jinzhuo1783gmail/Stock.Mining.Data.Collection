using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Symbol.Feature.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Manager
{
    public class UpdateScheduleManager
    {
        
        private ILogger<UpdateScheduleManager> _logger;
        private IConfiguration _configuration;
        private UpdateScheduleRepository _updateScheduleRepository;
        public UpdateScheduleManager(ILogger<UpdateScheduleManager> logger, IConfiguration configuration, UpdateScheduleRepository updateScheduleRepository)
        {
            _updateScheduleRepository = updateScheduleRepository;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<DateTime> GetUpdateScheduleAsync()
        {
            var updateSchedule = _updateScheduleRepository.Get();

            DateTime dateTime;

            _logger.LogInformation("request update schedule time");

            if (!DateTime.TryParse($"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {updateSchedule.UpdateTime}", out dateTime))
            {
                _logger.LogError($"Invalid schedule value [{updateSchedule.UpdateTime}]");
                return DateTime.MaxValue;
            }

            _logger.LogInformation($"return update schedule time is {dateTime}");

            return dateTime;
        }
    }
}
