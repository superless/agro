using Cosmonaut;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{

    /// <summary>
    /// Operaciones básicas para base de datos de persistencia
    /// 
    /// </summary>
    /// <typeparam name="T">Todos los elementos que heredan de documentbase son de base de datos de persistencia</typeparam>
    public interface IMainGenericDb<T> where T : DocumentBase {

        
        /// <summary>
        /// añade o actualiza un elemento en la base de datos.
        /// </summary>
        /// <param name="entity">entidad a añadir o actualizar</param>
        /// <returns>identificador de la entidad</returns>
        Task<string> CreateUpdate(T entity);


        /// <summary>
        /// Obtiene la entidad desde un id
        /// </summary>
        /// <param name="uniqueId">identificador del elemento</param>
        /// <returns>Retorna el elemento</returns>
        Task<T> GetEntity(string uniqueId);

        /// <summary>
        /// Borra una entidad
        /// </summary>
        /// <param name="id">identidicador de la entidad</param>
        /// <returns></returns>
        Task DeleteEntity(string id);


        /// <summary>
        /// Ejecuta consulta en la base de datos, asignando un tipo de valor esperado,
        /// esto puede ser un int, un string, un bool o incluso una entidad como Persona.
        /// las consultas deben obedecer al tipo de dato esperado.
        /// </summary>
        /// <typeparam name="T2">Tipo de valor esperado</typeparam>
        /// <param name="query">consulta a la base de datos</param>
        /// <param name="args">parámetros de la consulta</param>
        /// <returns>elemento de tipo T2</returns>
        Task<T2> SingleQuery<T2>(string query, params object[] args);


        /// <summary>
        /// Ejecuta una consulta en la base de datos que retornará multiples elementos de tipo T2
        /// donde T2 puede ser cualquier tipo que puediera devolver la base de datos,
        /// obedeciendo a la consulta como parámetro.
        /// </summary>
        /// <typeparam name="T2">Tipo a retornar, puede ser string, int u una entidad como Persona</typeparam>
        /// <param name="query">consulta</param>
        /// <param name="args">argumentos de esa consulta</param>
        /// <returns></returns>
        Task<IEnumerable<T2>> MultipleQuery<T2>(string query, params object[] args);
    }

}