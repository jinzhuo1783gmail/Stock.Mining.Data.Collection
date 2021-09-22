using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Util
{
    public static class Parser
    {
        public static decimal ConvertToDecimal(string? decimalStr, out string error, string type)
        {
            decimal decimalValue;

            if (!decimal.TryParse(decimalStr, out decimalValue))
            { 
                error =  $"Incorrect decimal value for column {type} value {decimalStr ?? ""}";
                return decimalValue;
            }

            error = "";
            return decimalValue;
        }

        public static DateTime ConvertToDatetime(string? datetimeStr, out string error, string type)
        {
            DateTime datetimeValue;

            if (!DateTime.TryParse(datetimeStr, out datetimeValue))
            { 
                error =  $"Incorrect decimal value for column {type} value {datetimeStr ?? ""}";
                return datetimeValue;
            }

            error = "";
            return datetimeValue;
        }
    }
}
