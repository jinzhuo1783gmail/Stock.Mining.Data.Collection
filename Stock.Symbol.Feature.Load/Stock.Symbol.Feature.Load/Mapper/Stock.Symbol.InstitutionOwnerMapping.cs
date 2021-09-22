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
    public static class InstitutionOwnerMapping
    {
        public static InstitutionOwner RapidJObjToModel(JToken ownership, out string error)
        {

            var institutionOwner = new InstitutionOwner();

            institutionOwner.ReportDate = Parser.ConvertToDatetime(ownership["reportDate"]["fmt"]?.ToString(), out error, "ReportDate");
            institutionOwner.InstitutionName = ownership["organization"]?.ToString();
            institutionOwner.Position = Parser.ConvertToDecimal(ownership["position"]["raw"]?.ToString(), out error, "Postion");
            institutionOwner.Percentage = Parser.ConvertToDecimal(ownership["pctHeld"]["raw"]?.ToString(), out error, "Percentage");
            institutionOwner.Value = Parser.ConvertToDecimal(ownership["value"]["raw"]?.ToString(), out error, "Value");

            return institutionOwner;
        }


        public static InstitutionOwner HoldingsChannelJObjToModel(JToken ownership, out string error)
        {

            var institutionOwner = new InstitutionOwner();

            var reportDate = ownership["&nbsp;As of"]?.ToString().Replace("&nbsp;", "") ?? "";

            reportDate = Regex.Replace(reportDate, @"\b(?<month>\d{1,2})/(?<day>\d{1,2})/(?<year>\d{2,4})\b", "${year}-${month}-${day}");

            institutionOwner.ReportDate = Parser.ConvertToDatetime(reportDate, out error, "ReportDate");
            institutionOwner.InstitutionName = ownership["Holder"]?.ToString();
            institutionOwner.Position = Parser.ConvertToDecimal(ownership["Position Size\n($ in 1000's)"]?.ToString().Replace("$", ""), out error, "Postion");
            institutionOwner.Percentage = 0;
            institutionOwner.Value = Parser.ConvertToDecimal(ownership["Amount"]?.ToString(), out error, "Value")　 * 1000 ;

            return institutionOwner;
        }

        public static InstitutionOwner TransactionsToModel(InstitutionTransaction transaction, out string error)
        {

            try
            {
                error = "";
                return new InstitutionOwner()
                {
                    InstitutionName = transaction.InstitutionName,
                    ReportDate = transaction.FileDate,
                    Position = transaction.Position,
                    Value = transaction.Value,
                    Percentage = 0
                };
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
            
        }

    }
}
