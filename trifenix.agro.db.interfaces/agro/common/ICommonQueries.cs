using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.agro.common
{
    public interface ICommonQueries {

        Task<string> GetSpecieAbbreviation(string idSpecie);

        Task<string> GetSpecieAbbreviationFromVariety(string idVariety);


        Task<string> GetSpecieAbbreviationFromBarrack(string idBarrack);

    }
}
