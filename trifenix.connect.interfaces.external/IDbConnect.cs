using Cosmonaut;
using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.graph;

namespace trifenix.connect.interfaces.external
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