using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.common
{
    public interface ICommonDbOperations<T>
    {
        Task<List<T>> TolistAsync(IQueryable<T> list);

        Task<T> FirstOrDefaultAsync(IQueryable<T> list, Expression<Func<T, bool>> predicate);

        IQueryable<T> WithPagination(IQueryable<T> list, int page, int totalElementsOnPage);

    }
}
