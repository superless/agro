using System.Collections.Generic;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.agro.common {
    public interface ICommonQueries {

        Task<string> GetSpecieAbbreviation(string idSpecie);

        Task<string> GetSpecieAbbreviationFromVariety(string idVariety);

        Task<string> GetSpecieAbbreviationFromBarrack(string idBarrack);

        Task<string> GetSpecieAbbreviationFromOrder(string idOrder);


        Task<List<string>> GetUsersMailsFromRoles(List<string> idsRoles);

        Task<string> GetSeasonId(string idBarrack);

        Task<string> GetUserIdFromAAD(string idAad);

    }
}