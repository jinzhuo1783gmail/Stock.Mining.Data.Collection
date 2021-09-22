using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension
{
    public static class HTMLReserveWordCleanUp
    {
        public static string ConvertHtmlChar(this String str)
        {
            return str.Replace("&amp", "&").Replace("&quot", "\"").Replace("&#039", "\'").Replace("&lt", "<").Replace("&gt", ">");
        }
    }
}
