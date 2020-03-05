using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.applicationsReference.agro.Common {
    public class CommonQueries : ICommonQueries {

        private readonly AgroDbArguments DbArguments;

        public CommonQueries(AgroDbArguments dbArguments) {
            DbArguments = dbArguments;
        }

        public async Task<string> GetSpecieAbbreviation(string idSpecie) {
            var db = new MainDb<Specie>(DbArguments);
            var result = await db.Store.QuerySingleAsync<string>($"SELECT value c.Abbreviation FROM c where  c.id = '{idSpecie}'");
            return result;
        }

        public async Task<string> GetSpecieAbbreviationFromVariety(string idVariety) {
            var db = new MainDb<Variety>(DbArguments);
            var idSpecie = await db.Store.QuerySingleAsync<string>($"SELECT value c.IdSpecie FROM c where  c.id = '{idVariety}'");
            return await GetSpecieAbbreviation(idSpecie);
        }

        public async Task<string> GetSpecieAbbreviationFromBarrack(string idBarrack) {
            var db = new MainDb<Barrack>(DbArguments);
            var result = await db.Store.QuerySingleAsync<string>($"SELECT value c.IdVariety FROM c where  c.id = '{idBarrack}'");
            return await GetSpecieAbbreviationFromVariety(result);
        }

        public async Task<string> GetSpecieAbbreviationFromOrder(string idOrder) {
            var db = new MainDb<ApplicationOrder>(DbArguments);
            var result = await db.Store.QuerySingleAsync<string>($"SELECT value c.IdBarrack[0] FROM c where  c.id = '{idOrder}'");
            return await GetSpecieAbbreviationFromBarrack(result);
        }

        public async Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles) {
            var db = new MainDb<User>(DbArguments);
            string query = $"SELECT DISTINCT value c.Email  FROM c join Rol in c.IdsRoles where Rol IN ({string.Join(",", idsRoles.Select(idRole => $"'{idRole}'").ToArray())})";
            var result = await db.Store.QueryMultipleAsync<string>(query);
            List<string> emails = result.ToList();
            return emails;
        }

        public async Task<string> GetSeasonId(string idBarrack) {
            var db = new MainDb<Barrack>(DbArguments);
            return await db.Store.QuerySingleAsync<string>($"SELECT value c.SeasonId FROM c where c.id = '{idBarrack}'");
        }

        public async Task<string> GetUserIdFromAAD(string idAAD) {
            var db = new MainDb<User>(DbArguments);
            return await db.Store.QuerySingleAsync<string>($"SELECT value c.id FROM c where c.ObjectIdAAD = '{idAAD}'");
        }

    }

}