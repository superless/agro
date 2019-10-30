using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.db.applicationsReference.common
{
    public class CommonDbOperations<T> : ICommonDbOperations<T>
    {
        public async Task<T> FirstOrDefaultAsync(IQueryable<T> list, Expression<Func<T, bool>> predicate)
        {
            if (list == null) return (T)(object)null;
            return await list.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> TolistAsync(IQueryable<T> list)
        {
            if (list == null) return new List<T>();
            return await list.ToListAsync();
        }
    }
}
