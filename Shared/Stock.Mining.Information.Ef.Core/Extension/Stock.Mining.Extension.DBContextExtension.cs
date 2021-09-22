using Microsoft.EntityFrameworkCore;
using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Extension
{
    public static class DBContextExtension
    {
        public static void DetachLocal<T>(this DbContext context, T t, long entryId) where T : class, IEntityBase 
    
            {
                var local = context.Set<T>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(entryId));
                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }
                context.Entry(t).State = EntityState.Modified;
            }
    }
}
