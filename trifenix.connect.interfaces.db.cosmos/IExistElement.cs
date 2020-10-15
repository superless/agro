using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{
    /// <summary>
    /// Interface para mantener todas las consultas a una base de datos o colección que determinen 
    /// la existencia de un elemento.
    /// </summary>
    public interface IExistElement {


        /// <summary>
        /// Devuelve true si el elemento ya existe.
        /// </summary>
        /// <typeparam name="T">Tipo de Clase que debe ir a buscar</typeparam>
        /// <param name="id">identificador a buscar</param>
        /// <returns>true si existe</returns>
        Task<bool> ExistsById<T>(string id) where T : DocumentBase;

        /// <summary>
        /// Determina si existe un elemento con cierta propiedad, asignando el nombre de la propiedad y el valor que debe chequear.
        /// si incluye el id, el método debería buscar en toda la base de datos, excepto el elemento con el id ingresado,
        /// si no verificará si existe en todos los elementos.
        /// El foco de este método es verificar.
        /// </summary>
        /// <typeparam name="T">Tipo de clase a buscar</typeparam>
        /// <param name="namePropCheck">nombre de la propiedad</param>
        /// <param name="valueCheck">valor de la propiedad</param>
        /// <param name="id">identificador del elemento, que no debe ser incluido</param>
        /// <returns>True si existe</returns>
        Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase;


      

    }

}