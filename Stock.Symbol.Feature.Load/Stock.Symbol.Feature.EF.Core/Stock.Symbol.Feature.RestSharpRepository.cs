using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stock.Mining.Information.Ef.Core.Extension;

namespace Stock.Symbol.Feature.EF.Core
{



    public class RestSharpRepository
    {

        public RSDBContext _context;
        public ILogger<RestSharpRepository> _logger;

        public RestSharpRepository(RSDBContext context, ILogger<RestSharpRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }


        public async Task<IList<RestSharpAccessKey>> GetAsync()
        {

            return await _context.RestSharpAccessKeys.AsNoTracking().ToListAsync();
        }

        public IList<RestSharpAccessKey> Get()
        {
            return _context.RestSharpAccessKeys.ToList();
        }

        public async Task<RestSharpAccessKey> GetAsync(int Id)
        {
            return await _context.RestSharpAccessKeys.FirstOrDefaultAsync(a => a.Id == Id);
        }




        public async Task<RestSharpAccessKey> GetAsync(string AccessKey)
        {
            return await _context.RestSharpAccessKeys.FirstOrDefaultAsync(a => a.AccessKey == AccessKey);
        }

        public async Task<RestSharpAccessKey> IncreaseCountAsync(int Id)
        {
            var key = await _context.RestSharpAccessKeys.FirstOrDefaultAsync(a => a.Id == Id);

            if (key == null)
            { 
                _logger.LogError($"Access key doesn't exist [{Id}]");
                return null;
            }

            key.MonthlyCount++;
            _context.Update(key);
            await _context.SaveChangesAsync();

            return key;
        }

        public async Task<RestSharpAccessKey> IncreaseCountAsync(string AccessKey, int resetDay)
        {

            var keys = (await FlushCountAsync(resetDay)).ToList();

            var key = keys.FirstOrDefault(a => a.AccessKey == AccessKey);

            if (key == null)
            { 
                _logger.LogError($"Access key doesn't exist [{AccessKey}]");
                return null;
            }
            

            key.MonthlyCount++;
            _context.Update(key);
            await _context.SaveChangesAsync();

            return key;
        }

        public async Task<IList<RestSharpAccessKey>> FlushCountAsync(int resetDay)
        { 

            var keys = await _context.RestSharpAccessKeys.ToListAsync();

            if (keys.Any(key => key.MonthlyCount > 0 && key.NextRefreshMonth <= (DateTime.Now.Year * 100 + DateTime.Now.Month) && resetDay == DateTime.Now.Day))
            { 
                keys.ForEach(k => { k.MonthlyCount = 0; k.NextRefreshMonth = DateTime.Now.AddMonths(1).Year * 100 + DateTime.Now.AddMonths(1).Month; });
                _context.RestSharpAccessKeys.UpdateRange(keys);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"All keys have been reset for month {DateTime.Now.Month} on Day {resetDay}");
            }

             return keys;
        }

    }
}
