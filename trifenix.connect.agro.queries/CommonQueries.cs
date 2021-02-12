using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.model_queries;
using trifenix.connect.agro_model;
using trifenix.connect.arguments;
using trifenix.connect.db.cosmos;

namespace trifenix.connect.agro.queries
{
    /// <summary>
    /// Consultas comunes a la base de datos
    /// </summary>
    public class CommonQueries : BaseQueries, ICommonAgroQueries {

        public CommonQueries(CosmosDbArguments dbArguments) : base(dbArguments) { }


        public string Queries(DbQuery query) => new Queries().Get(query);

        /// <summary>
        /// Obtener el e-mail de un usuario según su rol
        /// </summary>
        /// <param name="idsRoles"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles) {
            var result = await MultipleQuery<User, string>(Queries(DbQuery.MAILUSERS_FROM_ROLES),  string.Join(",", idsRoles.Select(idRole => $"'{idRole}'").ToArray()));
            List<string> emails = result.ToList();
            return emails;
        }
        /// <summary>
        /// Obtener la temporada de un cuartel segun su id
        /// </summary>
        /// <param name="idBarrack"></param>
        /// <returns></returns>
        public async Task<string> GetSeasonId(string idBarrack) => await SingleQuery<Barrack, string>(Queries(DbQuery.SEASONID_FROM_BARRACKID), idBarrack);

        /// <summary>
        /// Obtener el id de usuario segun su AAD(?
        /// </summary>
        /// <param name="idAAD"></param>
        /// <returns></returns>
        public async Task<string> GetUserIdFromAAD(string idAAD) => await SingleQuery<User, string>(Queries(DbQuery.USERID_FROM_IDAAD), idAAD);

        /// <summary>
        /// Obtener default dosis de un producto segun su id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public async Task<string> GetDefaultDosesId(string idProduct) => await SingleQuery<Dose, string>(Queries(DbQuery.DEFAULTDOSESID_BY_PRODUCTID), idProduct);

        /// <summary>
        /// Obtener dosis activas de un producto segun su id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetActiveDosesIdsFromProductId(string idProduct) => await MultipleQuery<Dose, string>(Queries(DbQuery.ACTIVEDOSESIDS_FROM_PRODUCTID), idProduct);

        /// <summary>
        /// Obtener business name asociado a un cost center, si es que existe
        /// </summary>
        /// <param name="idBusinessName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetBusinessNameIdFromCostCenter(string idBusinessName) => await MultipleQuery<CostCenter, string>(Queries(DbQuery.BUSINESSNAME_FROM_COSTCENTER), idBusinessName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IngredientId"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<string>> GetOrderFolderIngredientFromPreOrder(string IngredientId) => await MultipleQuery<OrderFolder, string>(Queries(DbQuery.ORDERFOLDERINGREDIENT_FROM_PREORDER), IngredientId);
    }

}