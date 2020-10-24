using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.log;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.interfaces.external
{

    /// <summary>
    /// Operaciones de guardado en base de datos de busqueda y persistencia
    /// las bases de datos de busqueda usan entitySearch.
    /// </summary>
    /// <typeparam name="T">tipo de dato de persistencia</typeparam>
    /// <typeparam name="T2">tipo de base de entrada de usuario</typeparam>
    public interface IGenericOperation<T,T2> : ILogSimpleQuery where T : DocumentBase where T2 : InputBase {

        /// <summary>
        /// obtiene un elemento de la base de datos de persistencia.
        /// Bajo el modelo, las lecturas se realizan bajo entitySearch
        /// </summary>
        /// <param name="id">identificador del elemento</param>
        /// <returns>Resultado Tipo Get con el elemento dentro</returns>
        Task<ExtGetContainer<T>> Get(string id);
        
        /// <summary>
        /// Valida el elemento de entrada de usuario.
        /// Normalmente se usará el validador automatizado.
        /// pero podrá poder implementar el propio.
        /// </summary>
        /// <param name="input">elemento de entrada de usuario</param>
        /// <returns></returns>
        Task Validate(T2 input);

        /// <summary>
        /// Guarda una entidad en la base de datos, si el id no existe, inserta, sino actualiza.
        /// </summary>
        /// <param name="entity">entidad de base de datos de persistencia a guardar</param>
        /// <returns></returns>
        Task<ExtPostContainer<string>> SaveDb(T entity);


        /// <summary>
        /// Guarda elemento de base de datos de persistencia en una base de datos de entitySearch,
        /// esto implica convertir el elemento de base de datos en uno o más entitySearch antes de guardar.
        /// </summary>
        /// <param name="entity">entidad de base de datos de persistencia</param>
        /// <returns>Resultado Post con el identificador del elemento creado</returns>
        Task<ExtPostContainer<string>> SaveSearch(T entity);

        /// <summary>
        /// Guarda una entidad ingresada por usuario,
        /// por tanto debe
        /// 1. Validar.
        /// 2. Convertir en elemento de base de datos de persitencia.
        /// 3. Guardar en la base de datos de persistencia
        /// 4. Convertir el elemento de base de datos de persistencia en uno o más entitySearch.
        /// 5. Guardar en base de datos de busqueda basada en entitySearch.
        /// 
        /// Como se puede apreciar, se guarda primero en la base de datos de persistencia, si falla no lo guardará en la base de datos de busqueda
        /// </summary>
        /// <param name="entityInput">entidad ingresada por usuario</param>
        /// <returns>PostResult con el identificador del elemento guardado</returns>
        Task<ExtPostContainer<string>> SaveInput(T2 entityInput);

        /// <summary>
        /// Elimina un elemento de la base de datos de persistencia y de busqueda (entitySearch)
        /// </summary>
        /// <param name="id">identificador del elemento a eliminar</param>
        /// <returns></returns>
        Task Remove(string id);
        


    }

}