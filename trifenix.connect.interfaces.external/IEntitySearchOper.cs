using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.interfaces.external
{
    public interface IEntitySearchOper<T> {
        IEntitySearch<T>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentBase;

        IEntitySearch<T>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase;

        void AddDocument<T2>(T2 document) where T2 : DocumentBase;

        

    }

}