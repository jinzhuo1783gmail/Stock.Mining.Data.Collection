using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public class MiningRespository : IMiningRespository
    {
        public InformationDBContext _context;
        public async Task<bool> SaveContextAsync()
        {
            var updated = await _context.SaveChangesAsync();
            return updated > 0 ;
        }
    }
}
