using trifenix.connect.agro.interfaces.external.util;
using trifenix.connect.agro.interfaces.search;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.search;

namespace trifenix.connect.agro.interfaces.external
{

    public interface IAgroSearch<T> : IRelatedSearch<T>, IEntitySearchOper<T>, IBaseEntitySearch<T> { 

    }

}