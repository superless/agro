using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.external.operations.tests.helper.staticHelper
{
    public static class MoqCommonDb
    {
        public static Mock<ICommonDbOperations<T>> GetDbOperationsReturnResult<T>()
        {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();
            Func<IQueryable<T>, List<T>> fnc = (s) => s == null ? new List<T>() : s.ToList();
            mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync(fnc);
            Func<IQueryable<T>, Expression<Func<T, bool>>, T> fncElement = (elements, expresion) => elements.FirstOrDefault();
            mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(fncElement);
            return mockCommonDb;
        }

        public static Mock<ICommonDbOperations<T>> GetDbOperationsReturnNull<T>() {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();
            mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync((List<T> )null);
            mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(null);
            return mockCommonDb;
        }

        public static Mock<ICommonDbOperations<T>> GetDbOperationsReturnEmpty<T>()
        {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();
            mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync(new List<T>());
            mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(null);
            return mockCommonDb;
        }

        public static Mock<ICommonDbOperations<T>> GetDbOperationsThrowException<T>()
        {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();
            mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).Throws(new Exception("simulated test error")); 
            mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).Throws(new Exception("simulated test error")); 
            return mockCommonDb;
        }




    }
}
