using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension.Mapper
{
     public static class InstitutionHoldingHistoriesMapper
    {

        public static InstitutionTransaction ToInstitutionHoldingTransactionModel(this InstitutionHoldingsHistory institutionHoldingsHistory)
        {
            return new InstitutionTransaction()
                            {
                                Id = institutionHoldingsHistory?.Id ?? default(long),
                                InstitutionOwnerId = institutionHoldingsHistory?.InstitutionId ?? default(long),
                                FileDate = institutionHoldingsHistory?.FileDate ?? default(DateTime),
                                InstitutionName = institutionHoldingsHistory?.InstitutionHolding?.InstitutionName,
                                Price = (institutionHoldingsHistory?.Postion > 0 ? institutionHoldingsHistory.Value / institutionHoldingsHistory.Postion : 0),
                                Position = institutionHoldingsHistory?.Postion ?? default(decimal),
                                Value = institutionHoldingsHistory?.Value ?? default(decimal)
                            };
        }


        public static InstitutionHoldingsHistory ToInstitutionHoldingHistoryEntity(this InstitutionTransaction institutionHoldingsTransaction, long institutionId = default(long), InsitutionHoldingTrend trend = InsitutionHoldingTrend.Hold)
        {
            return new InstitutionHoldingsHistory()
            {
                Id = institutionHoldingsTransaction?.Id ?? default(long), 
                InstitutionId = institutionHoldingsTransaction?.InstitutionOwnerId == null || institutionHoldingsTransaction?.InstitutionOwnerId == default(long) ? institutionId : institutionHoldingsTransaction.InstitutionOwnerId,
                FileDate = institutionHoldingsTransaction?.FileDate ?? default(DateTime),
                Postion = institutionHoldingsTransaction?.Position ?? default(decimal),
                Value = institutionHoldingsTransaction?.Value ?? default(decimal),
                Action = trend.ToString() // Update later
            };
        }

        public static InstitutionHolding ToInstitutionHoldingEntity(this InstitutionTransaction institutionHoldingsTransaction, long symbolId)
        {
            
            var institutionHolding = new InstitutionHolding()
            {
                Id = institutionHoldingsTransaction?.Id ?? default(long),
                
                SymbolId = symbolId,
                InstitutionName = institutionHoldingsTransaction?.InstitutionName,
                MatchWord = string.Join(' ', institutionHoldingsTransaction?.InstitutionName.Split(" ").Take(2)),
                ReportDate = institutionHoldingsTransaction?.FileDate ?? default(DateTime),
                Postion = institutionHoldingsTransaction?.Position ?? default(decimal),
                Value = institutionHoldingsTransaction?.Value ?? default(decimal),
                Percentage = default(decimal)
            };

            return institutionHolding;
        }
    }
}
