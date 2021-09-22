using Newtonsoft.Json.Linq;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Extension;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Mapper
{
    public static class InstitutionTransactionMapping
    {
        public static InstitutionTransaction HtmlJObjToModel(JToken transaction, out string error)
        {

            var institutionTransaction = new InstitutionTransaction();

            institutionTransaction.FileDate = Parser.ConvertToDatetime(transaction["File Date"]?.ToString(), out error, "FileDate");
            institutionTransaction.Source = transaction["Source"]?.ToString().Replace("\n", "");
            institutionTransaction.InstitutionName = transaction["Investor"]?.ToString().Replace("\n", "") ?? "" ;
            if (! string.IsNullOrEmpty(institutionTransaction.InstitutionName) && institutionTransaction.InstitutionName.Contains("[click"))
            {
                int charLocation = institutionTransaction.InstitutionName.IndexOf("[click", StringComparison.Ordinal);
                institutionTransaction.InstitutionName = institutionTransaction.InstitutionName.Substring(0, charLocation);
            }

            institutionTransaction.InstitutionName = institutionTransaction.InstitutionName.ConvertHtmlChar();
               
            institutionTransaction.Option = transaction["Opt"]?.ToString().Replace("\n", "");
            institutionTransaction.Price = Parser.ConvertToDecimal(transaction["Avg Share Price"]?.ToString(), out error, "Price");
            institutionTransaction.Position = Parser.ConvertToDecimal(transaction["Shares"]?.ToString(), out error, "Position");
            institutionTransaction.ShareChanged = Parser.ConvertToDecimal(transaction["Shares Changed(%)"]?.ToString(), out error, "ShareChanged");

            institutionTransaction.Value = Parser.ConvertToDecimal(transaction["Value($1000)"]?.ToString(), out error, "Value")　 * 1000;
            institutionTransaction.ValueChanged = Parser.ConvertToDecimal(transaction["Value Changed(%)"]?.ToString(), out error, "ValueChanged");


            return institutionTransaction;
        }

    }
}
