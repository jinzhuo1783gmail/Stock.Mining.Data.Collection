using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public interface IMiningRespository
    {
        Task<bool> SaveContextAsync();

    }
}
