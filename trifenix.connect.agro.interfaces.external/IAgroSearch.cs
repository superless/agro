using trifenix.connect.agro.interfaces.search;
using trifenix.connect.interfaces.search;

namespace trifenix.connect.agro.interfaces.external
{

    public interface IAgroSearch<T> : IRelatedAgroSearch<T>, IBaseEntitySearch<T> { 

    }

}