using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class ApiSymbol
    {
        public long Id { get; set; }
        public string GetAllSymbol { get; set; }
        public string UpsertSymbol { get; set; }
        public string DisableScan { get; set; }

    }
}
