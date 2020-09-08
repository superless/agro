using Cosmonaut;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.microsoftgraph.operations;
using trifenix.connect.agro_model;

namespace trifenix.agro.external.operations
{
    /// <summary>
    /// Enlaces a base de datos, para las distintas operaicones
    /// </summary>
    public class DbConnect : IDbConnect
    {
        public DbConnect(AgroDbArguments arguments)
        {
            Arguments = arguments;
        }

        // argumentos de base de datos
        public AgroDbArguments Arguments { get; }


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

        // 
        public IGraphApi GraphApi => new GraphApi(Arguments);
    }

}