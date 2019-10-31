using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class CommonDbInstances<T> {

        public static Mock<ICommonDbOperations<T>> GetInstance(Results result)
        {
            var mockCommonDb = new Mock<ICommonDbOperations<T>>();
            switch (result)
            {
                case Results.Nullables:
                    mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync((List<T>)null);
                    mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(null);
                    break;
                case Results.Empty:
                    mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync(new List<T>());
                    mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(null);
                    break;
                case Results.Errors:
                    mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).Throws(new Exception("simulated test error"));
                    mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).Throws(new Exception("simulated test error"));
                    break;
                case Results.Values:
                    Func<IQueryable<T>, List<T>> fnc = (s) => s == null ? new List<T>() : s.ToList();
                    mockCommonDb.Setup(s => s.TolistAsync(It.IsAny<IQueryable<T>>())).ReturnsAsync(fnc);
                    Func<IQueryable<T>, Expression<Func<T, bool>>, T> fncElement = (elements, expresion) => elements.FirstOrDefault();
                    mockCommonDb.Setup(s => s.FirstOrDefaultAsync(It.IsAny<IQueryable<T>>(), It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(fncElement);
                    break;
                default:
                    break;
            }
            return mockCommonDb;
        }
    }
}
