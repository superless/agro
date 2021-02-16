using System.Collections.Generic;
using System.Threading.Tasks;

namespace trifenix.connect.agro.interfaces
{
    /// <summary>
    /// Consultas comunes a base de datos, para agroFenix
    /// </summary>
    public interface ICommonAgroQueries {

        
        /// <summary>
        /// Obtiene los corroeos desde los roles.
        /// </summary>
        /// <param name="idsRoles">identificador de roles</param>
        /// <returns></returns>
        Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles);

        /// <summary>
        /// obtiene el season id desde barrack
        /// </summary>
        /// <param name="idBarrack">identificador de barrack</param>
        /// <returns></returns>
        Task<string> GetSeasonId(string idBarrack);


        /// <summary>
        /// Obtiene el identificador de usuario desde el id de active directory.
        /// </summary>
        /// <param name="idAAD">identificador de active directory</param>
        /// <returns>identificador del usuario</returns>
        Task<string> GetUserIdFromAAD(string idAAD);

        /// <summary>
        /// Obtiene la dosis por defecto de un producto
        /// </summary>
        /// <param name="idProduct">identificador de producto, donde buscar la dosis</param>
        /// <returns></returns>
        Task<string> GetDefaultDosesId(string idProduct);


        /// <summary>
        /// Obtiene las dosis activas de un producto
        /// </summary>
        /// <param name="idProduct">identificador del producto</param>
        /// <returns>identificadores de las dosis activas</returns>
        Task<IEnumerable<string>> GetActiveDosesIdsFromProductId(string idProduct);

        /// <summary>
        /// Obtiene el business name asociado al idBusinessName de un cost center, si es que existe
        /// </summary>
        /// <param name="idBusinessName"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetBusinessNameIdFromCostCenter(string idBusinessName);

        /// <summary>
        /// Obtiene el ingrediente de la order folder desde el order folder de la pre orden
        /// </summary>
        /// <param name="OrderFolderId"></param>
        /// <returns></returns>
        Task<string> GetOrderFolderIngredientFromPreOrder(string OrderFolderId);

        /// <summary>
        /// Obtiene los ingredientes de todas las pre ordenes de una older folder
        /// </summary>
        /// <param name="OrderFolderId"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetPreOrderIngredientFromOrderFolder(string OrderFolderId);

        /// <summary>
        /// Comprueb si existen order folders duplicadas
        /// </summary>
        /// <param name="ApplicationTargetId"></param>
        /// <param name="IngredientId"></param>
        /// <param name="PhenologicalEventId"></param>
        /// <param name="SpecieId"></param>
        /// <returns></returns>
        Task<string> GetDuplicatedOrderFolders(string ApplicationTargetId, string IngredientId, string PhenologicalEventId, string SpecieId);

        /// <summary>
        /// Obtiene la temporada activa si es que existe
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetActiveSeason();
        
        /// <summary>
        /// Obtiene el estado de una temporada 
        /// </summary>
        /// <param name="idSeason"></param>
        /// <returns></returns>
        Task<string> GetSeasonStatus(string idSeason);
    }

}