using Newtonsoft.Json.Linq;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Mapper
{
    public static class InsiderTransactionMapping
    {
         public static InsiderTransaction RapidJObjToModel(JToken transactionRaw, out string error)
        {



            var insiderTransaction = new InsiderTransaction();

            insiderTransaction.TransactionDate = Parser.ConvertToDatetime(transactionRaw["startDate"]["fmt"]?.ToString(), out error, "TransactionDate");
            insiderTransaction.HolderName = transactionRaw["filerName"].ToString();
            insiderTransaction.Role = transactionRaw["filerRelation"].ToString();
            insiderTransaction.Shares = Parser.ConvertToDecimal(transactionRaw["shares"]["raw"]?.ToString(), out error, "Shares");
            insiderTransaction.Value = Parser.ConvertToDecimal(transactionRaw["value"]["raw"]?.ToString(), out error, "Value");
            insiderTransaction.Description = transactionRaw["transactionText"].ToString();
            insiderTransaction.Side = (Regex.IsMatch(insiderTransaction.Description.ToLower(), @"sell|sale") ? "Sell" : (Regex.IsMatch(insiderTransaction.Description.ToLower(), @"purchase") ? "Buy" : "Other"));
        

            return insiderTransaction;
        }
    }
}
