using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.Common
{
    public class CommonQueries : ICommonQueries
    {
        private AgroDbArguments DbArguments;

        public CommonQueries(AgroDbArguments dbArguments)
        {
            DbArguments = dbArguments;
        }
        public async Task<string> GetSpecieAbbreviation(string idSpecie)
        {
            var db = new MainDb<Specie>(DbArguments);
            var result = await db.Store.QuerySingleAsync<string>($"SELECT value c.Abbreviation FROM c where  c.Id = '{idSpecie}'");

            return result;
        }

        public async Task<string> GetSpecieAbbreviationFromBarrack(string idBarrack)
        {
            var db = new MainDb<Barrack>(DbArguments);
            var result = await db.Store.QuerySingleAsync<string>($"SELECT value c.IdVariety FROM c where  c.Id = '{idBarrack}'");
            return await GetSpecieAbbreviationFromVariety(result);
        }

        public async Task<string> GetSpecieAbbreviationFromVariety(string idVariety)
        {
            var db = new MainDb<Variety>(DbArguments);
            var idSpecie = await db.Store.QuerySingleAsync<string>($"SELECT value c.IdSpecie FROM c where  c.Id = '{idVariety}'");

            return await GetSpecieAbbreviation(idSpecie);


        }
    }
}
