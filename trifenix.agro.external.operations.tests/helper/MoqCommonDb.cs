using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqCommonDb
    {
        public Mock<ICommonDbOperations<T>> GetDbOperations<T>()
        {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();

            Func<IQueryable<T>, List<T>> fnc = (s) => s == null ? new List<T>() : s.ToList();


            mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync(fnc);

            //Func<IQueryable<T>, Expression<Func<T, bool>>, T> fncElement = (elements, expresion) => elements.FirstOrDefault();

            //mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(fncElement);
            mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(null);

            return mockCommonDb;

        }

    }
}
