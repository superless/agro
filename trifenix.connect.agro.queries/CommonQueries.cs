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

    public class CommonQueries : BaseQueries, ICommonAgroQueries {

        public CommonQueries(CosmosDbArguments dbArguments) : base(dbArguments) { }


        public string Queries(DbQuery query) => new Queries().Get(query);

        public async Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles) {
            var result = await MultipleQuery<User, string>(Queries(DbQuery.MAILUSERS_FROM_ROLES),  string.Join(",", idsRoles.Select(idRole => $"'{idRole}'").ToArray()));
            List<string> emails = result.ToList();
            return emails;
        }

        public async Task<string> GetSeasonId(string idBarrack) => await SingleQuery<Barrack, string>(Queries(DbQuery.SEASONID_FROM_BARRACKID), idBarrack);

        public async Task<string> GetUserIdFromAAD(string idAAD) => await SingleQuery<User, string>(Queries(DbQuery.USERID_FROM_IDAAD), idAAD);

        public async Task<string> GetDefaultDosesId(string idProduct) => await SingleQuery<Dose, string>(Queries(DbQuery.DEFAULTDOSESID_BY_PRODUCTID), idProduct);

        public async Task<IEnumerable<string>> GetActiveDosesIdsFromProductId(string idProduct) => await MultipleQuery<Dose, string>(Queries(DbQuery.ACTIVEDOSESIDS_FROM_PRODUCTID), idProduct);

        

    }

}