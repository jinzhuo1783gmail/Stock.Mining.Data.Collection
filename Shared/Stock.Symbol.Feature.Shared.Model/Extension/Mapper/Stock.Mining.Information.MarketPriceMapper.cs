using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension.Mapper
{
    public static class MarketPriceMapper
    {

        public static StockPriceViewModel ToMarketPriceModel(this SymbolPrice symbolPrice)
        {
            
                var price =  new StockPriceViewModel()
                {
                    Id = symbolPrice.Id,
                    SymbolId = symbolPrice.SymbolId,
                    PriceDate = symbolPrice.PriceDate,
                    Open = symbolPrice.Open,
                    High = symbolPrice.High,
                    Low = symbolPrice.Low,
                    Close = symbolPrice.Close,
                    AdjustClose = symbolPrice.AdjustClose,
                    Volume = symbolPrice.Volume,
                    SplitCoefficient = symbolPrice.SplitCoefficient
                };

            
            return price;
        }


        public static SymbolPrice ToMarketPriceEntity(this StockPriceViewModel stockPrice, long symbolId = default(long))
        {
            
                var price =  new SymbolPrice()
                {
                    Id = stockPrice.Id,
                    SymbolId = stockPrice.SymbolId == default(long) ? symbolId :  stockPrice.SymbolId,
                    PriceDate = stockPrice.PriceDate,
                    Open = stockPrice.Open,
                    High = stockPrice.High,
                    Low = stockPrice.Low,
                    Close = stockPrice.Close,
                    AdjustClose = stockPrice.AdjustClose,
                    Volume = stockPrice.Volume,
                    SplitCoefficient = stockPrice.SplitCoefficient
                };

            
            return price;
        }

    }
}
