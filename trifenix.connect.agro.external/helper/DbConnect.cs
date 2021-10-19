using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.queries;
using trifenix.connect.arguments;
using trifenix.connect.db.cosmos;
using trifenix.connect.graph;
using trifenix.connect.input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;
using trifenix.connect.model;

namespace trifenix.connect.agro.external.helper
{
    /// <summary>
    /// Enlaces a base de datos, para las distintas operaciones
    /// </summary>
    public class DbConnect : IDbAgroConnect
    {
        /// <summary>
        /// argumentos para cosmosdb
        /// </summary>
        /// <param name="arguments">argumentos de cosmosdb</param>
        public DbConnect(CosmosDbArguments arguments)
        {
            Arguments = arguments;
        }

        /// <summary>
        /// Argumentos de la base de datos.
        /// </summary>
        public CosmosDbArguments Arguments { get; }


        
        // consultas comunes.
        public ICommonAgroQueries CommonQueries => new CommonQueries(Arguments);


        // Elementos en existencia.
        public IExistElement ExistsElements(bool isBatch=false) => (IExistElement)new CosmosExistElement(Arguments);


        // Operaciones comunes en la base de datgos
        


        // Operaciones comunes en la base de datos (CRUD).
        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentDb
        {
            return new MainGenericDb<T>(Arguments);
        }

        public IValidatorAttributes<T_INPUT> GetValidator<T_INPUT, T_DB>()
            where T_INPUT : InputBase
            where T_DB : DocumentDb
        {
            return new MainValidator<T_DB, T_INPUT>(GetDbExistsElements);
        }



        // 
        public IGraphApi GraphApi => new GraphApi();

        public IDbExistsElements GetDbExistsElements => new CosmosExistElement(Arguments);
    }

}