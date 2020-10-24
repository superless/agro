using Cosmonaut;
using trifenix.connect.agro.interfaces;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.graph;

namespace trifenix.connect.interfaces.external
{
    /// <summary>
    /// Elementos necesarios para operaciones crud en base de datos.
    /// </summary>
    public interface IDbConnect {


        /// <summary>
        /// Elemento principal de base de datos
        /// </summary>
        /// <typeparam name="T">elemento de base de datos de persistencia</typeparam>
        /// <returns>Reposirio de base de datos para tipo T</returns>
        IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase;

        /// <summary>
        /// Operaciones de base de datos comunes, esto es dependiente de CosmosDb
        /// Dado que el retorno es un IQueryable que usa un método estático para convertirlo en lista
        /// estos métodos son utilizados para convertirlos en listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase;

        /// <summary>
        /// Api de identidades de Microsoft.
        /// </summary>
        IGraphApi GraphApi { get; }


        // Validaciones de input, usando los atributos del modelo.
        IValidatorAttributes<T_INPUT> GetValidator<T_INPUT, T_DB>() where T_INPUT : InputBase where T_DB : DocumentBase;

        


    }

}