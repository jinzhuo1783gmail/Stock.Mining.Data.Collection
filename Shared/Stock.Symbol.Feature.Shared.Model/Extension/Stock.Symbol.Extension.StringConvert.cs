using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension
{
    public static class StringConvert
    {
        public static InsitutionHoldingTrend ToInsitutionHoldingTrend(this string trend, out string error)
        {
            error = "";
            if (string.IsNullOrEmpty(trend))
                return InsitutionHoldingTrend.New;

            if (Enum.TryParse<InsitutionHoldingTrend>(trend, true, out var insitutionHoldingTrend))
                return insitutionHoldingTrend;
            else
            {
                error = $"Invalid value for InsitutionHoldingTrend {trend}";
                return InsitutionHoldingTrend.Error;
            }    


        }

    }
}
