using System.Collections.Generic;

namespace trifenix.agro.search.interfaces {
    public interface IAgroSearch {

        void AddElements<T>(List<T> elements);
        void DeleteElements<T>(List<T> elements);

    }
}