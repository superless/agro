using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db;

namespace trifenix.agro.external.operations.tests.helper.Moqs
{
    public static class MoqGenerator
    {
        public static Mock<T> GetMoqResult<T, T2>(Results result, Expression<Func<T, Task<string>>> create, Expression<Func<T, Task<T2>>> getElement, Expression<Func<T, IQueryable<T2>>> getElements ) where T : class where T2 : DocumentBase
        {
            var mock = new Mock<T>();
           

            

            switch (result)
            {
                case Results.Nullables:
                    mock.Setup(create).ReturnsAsync((string)null);
                    mock.Setup(getElement).ReturnsAsync((T2)null);
                    mock.Setup(getElements).Returns((IQueryable<T2>)null);
                    break;
                case Results.Empty:
                    mock.Setup(create).ReturnsAsync(string.Empty);
                    mock.Setup(getElement).ReturnsAsync((T2)null);
                    mock.Setup(getElements).Returns(new List<T2>().AsQueryable());
                    break;
                case Results.Errors:
                    mock.Setup(create).Throws(new Exception("simulated test error"));
                    mock.Setup(getElement).Throws(new Exception("simulated test error"));
                    mock.Setup(getElements).Throws(new Exception("simulated test error"));
                    break;
                case Results.Values:
                    mock.Setup(create).ReturnsAsync(FakeGenerator.CreateString());
                    Func<string, T2> fnc = s => FakeGenerator.GetElement<T2>(s);
                    mock.Setup(getElement).ReturnsAsync(fnc);
                    mock.Setup(getElements).Returns(FakeGenerator.GetElements<T2>());
                    break;
                default:
                    break;
            }
            
            return mock;
        }

    }
}
