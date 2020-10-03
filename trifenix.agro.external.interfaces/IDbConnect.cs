using Cosmonaut;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.external.interfaces
{
    public interface IDbConnect {

        IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase;

        ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase;

        ICosmosStore<EntityContainer> BatchStore { get; }

        IExistElement ExistsElements(bool isBatch);

        ICommonQueries CommonQueries { get; }

        IGraphApi GraphApi { get; }

        IValidatorAttributes<T_INPUT, T_DB> GetValidator<T_INPUT, T_DB>(bool isBatch) where T_INPUT : InputBase where T_DB : DocumentBase;

    }

}