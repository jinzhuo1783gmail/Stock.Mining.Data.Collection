using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension.Mapper
{
    public static class InsititionHoldingsMapper
    {
        public static InstitutionOwner ToInstitutionHoldingModel(this InstitutionHolding institutionHolding, bool includeSubEntity = false)
        {
            
                var isOwner =  new InstitutionOwner()
                {
                    Id = institutionHolding?.Id ?? default(long),
                    SymbolId = institutionHolding?.SymbolId ?? default(long),
                    InstitutionName = institutionHolding?.InstitutionName,
                    Percentage = institutionHolding?.Percentage ?? default(decimal),
                    Position = institutionHolding?.Postion ?? default(decimal),
                    Value = institutionHolding?.Value ?? default(decimal),
                    ReportDate = institutionHolding?.ReportDate ?? default(DateTime)
                };

            if (includeSubEntity)
            {
                isOwner.InstitutionHoldingsHistories = institutionHolding.InstitutionHoldingsHistories.Select(ih => ih.ToInstitutionHoldingTransactionModel()).ToList();
            }

            return isOwner;
        }

       

        public static InstitutionHolding ToInstitutionHoldingEntity(this InstitutionOwner institutionOwner, long symbolId = default(long), bool includeSubEntity = false)
        {
            var institutionHolding = new InstitutionHolding()
            {
                Id = institutionOwner?.Id ?? default(long),
                SymbolId = (institutionOwner != null && institutionOwner.SymbolId != default(long))  ? institutionOwner.SymbolId : symbolId,
                InstitutionName = institutionOwner?.InstitutionName,
                MatchWord = string.IsNullOrEmpty(institutionOwner?.MatchWord) ?  string.Join(' ', institutionOwner?.InstitutionName.Split(" ").Take(2)) : institutionOwner?.MatchWord,
                ReportDate = institutionOwner?.ReportDate ?? default(DateTime),
                Postion = institutionOwner?.Position ?? default(decimal),
                Value = institutionOwner?.Value ?? default(decimal),
                Percentage = institutionOwner?.Percentage ?? default(decimal)
            };

            if (includeSubEntity && institutionOwner.InstitutionHoldingsHistories != null)
            {
                institutionHolding.InstitutionHoldingsHistories = institutionOwner.InstitutionHoldingsHistories.Select(ihh => ihh.ToInstitutionHoldingHistoryEntity()).ToList();

            }

            return institutionHolding;

        }

    }
}
