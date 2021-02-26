using System.Collections.Generic;
using System.Threading.Tasks;

namespace trifenix.connect.agro.interfaces
{
    /// <summary>
    /// Consultas comunes a base de datos, para agroFenix
    /// </summary>
    public interface ICommonAgroQueries {

        
        /// <summary>
        /// Obtiene los correos desde los roles.
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
        Task<IEnumerable<string>> GetCostCenterFromBusinessName(string idBusinessName);

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
        /// <param name="IdCostCenter"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetCostCenterActiveSeason(string IdCostCenter);
        
        /// <summary>
        /// Obtiene el estado de una temporada 
        /// </summary>
        /// <param name="idSeason"></param>
        /// <returns></returns>
        Task<string> GetSeasonStatus(string IdSeason);

        /// <summary>
        /// Obtiene todos los barracks asociados a la order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        Task<IEnumerable<IEnumerable<string>>> GetBarracksFromOrderFolderId(string IdOrderFolder);

        /// <summary>
        /// Obtiene la variedad de un barrack
        /// </summary>
        /// <param name="IdBarrack"></param>
        /// <returns></returns>
        Task<string> GetBarrackVarietyFromBarrackId(string IdBarrack);

        /// <summary>
        /// Obtiene la especie de una variedad
        /// </summary>
        /// <param name="IdVariety"></param>
        /// <returns></returns>
        Task<string> GetSpecieFromVarietyId(string IdVariety);

        /// <summary>
        /// Obtiene la especie de la order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        Task<string> GetOFSpecie(string IdOrderFolder);

        /// <summary>
        /// Obtiene los atributos de una order folder
        /// </summary>
        /// <param name="IdOrderFolder"></param>
        /// <returns></returns>
        Task<IEnumerable<Dictionary<string,string>>> GetOFAttributes(string IdOrderFolder);

        /// <summary>
        /// Obtiene las order folder que tengan el mismo evento fenológico, el mismo objetivo de aplicación y la misma especie
        /// </summary>
        /// <param name="IdPhenologicalEvent"></param>
        /// <param name="IdApplicationTarget"></param>
        /// <param name="IdSpecie"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetSimilarOF(string IdPhenologicalEvent, string IdApplicationTarget, string IdSpecie);

        /// <summary>
        /// Obtiene los barracks de una misma order folder
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<IEnumerable<string>>> GetOFBarracks(string Id);
    }

}