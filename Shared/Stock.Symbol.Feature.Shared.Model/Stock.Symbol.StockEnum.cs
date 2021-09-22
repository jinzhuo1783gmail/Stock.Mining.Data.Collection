using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public enum InsitutionHoldingTrend
    { 
        None = 0,
        New,
        Increase,
        Hold,
        Decrease,
        Liquidated,
        Error,

    }
}
