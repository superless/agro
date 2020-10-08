using Cosmonaut;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.queries;
using trifenix.connect.db;
using trifenix.connect.db.cosmos;
using trifenix.connect.entities.cosmos;
using trifenix.connect.graph;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;

namespace trifenix.connect.agro.external.helper
{
    /// <summary>
    /// Enlaces a base de datos, para las distintas operaicones
    /// </summary>
    public class DbConnect : IDbAgroConnect
    {
        public DbConnect(CosmosDbArguments arguments)
        {
            Arguments = arguments;
        }

        // argumentos de base de datos
        public CosmosDbArguments Arguments { get; }


        // batchstore usado para realizar operaciones en batch en la base de datos.
        public ICosmosStore<EntityContainer> BatchStore =>   new CosmosStore<EntityContainer>(new CosmosStoreSettings(Arguments.NameDb, Arguments.EndPointUrl, Arguments.PrimaryKey));

        // consultas comunes.
        public ICommonQueries CommonQueries => new CommonQueries(Arguments);


        // Elementos en existencia.
        public IExistElement ExistsElements(bool isBatch) => isBatch? (IExistElement) new BatchExistsElements(Arguments) : new CosmosExistElement(Arguments);


        // Operaciones comunes en la base de datgos
        public ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase => new CommonDbOperations<T>();


        // Operaciones comunes en la base de datos (CRUD).
        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase
        {
            return new MainGenericDb<T>(Arguments);
        }

        public IValidatorAttributes<T_INPUT, T_DB> GetValidator<T_INPUT, T_DB>(bool isBatch)
            where T_INPUT : InputBase
            where T_DB : DocumentBase
        {
            return new MainValidator<T_DB, T_INPUT>(ExistsElements(isBatch));
        }



        // 
        public IGraphApi GraphApi => new GraphApi(Arguments);

        public IDbExistsElements GetDbExistsElements => new CosmosExistElement(Arguments);
    }

}