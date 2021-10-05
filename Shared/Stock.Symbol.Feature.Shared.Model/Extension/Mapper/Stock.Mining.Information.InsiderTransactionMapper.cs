using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model.Extension.Mapper
{
    public static class InsiderTransactionMapper
    {
        public static InsiderHistory ToInsiderHistoryEntity(this InsiderTransaction insiderTransaction, long symbolId = default(long))
        {
            
                var insiderHistory =  new InsiderHistory()
                {
                   
                    SymbolId = symbolId,
                    TransactionDate = insiderTransaction.TransactionDate,
                    HolderName = insiderTransaction.HolderName,
                    Shares = insiderTransaction.Shares,
                    Value = insiderTransaction.Value,
                    Side = insiderTransaction.Side,
                    Description = insiderTransaction.Description,
                    Role = insiderTransaction.Role,

                };
            
            return insiderHistory;
        }

        public static InsiderTransaction ToInsiderTransactionModel(this InsiderHistory insiderHisitory)
        {
            
                var insiderTransaction =  new InsiderTransaction()
                {

                    TransactionDate = insiderHisitory.TransactionDate,
                    HolderName = insiderHisitory.HolderName,
                    Shares = insiderHisitory.Shares,
                    Value = insiderHisitory.Value,
                    Side = insiderHisitory.Side,
                    Description = insiderHisitory.Description,
                    Role = insiderHisitory.Role,

                };
            
            return insiderTransaction;
        }

    }
}
