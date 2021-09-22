using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Symbol.Feature.EF.Core
{
    public class AlphaVantageRepository
    {

        public RSDBContext _context;
        public ILogger<AlphaVantageRepository> _logger;

        public AlphaVantageRepository(RSDBContext context, ILogger<AlphaVantageRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public IList<AlphaVantageAccessKey> Get()
        {
            return _context.AlphaVantageAccessKeys.ToList();
        }
    }
}
