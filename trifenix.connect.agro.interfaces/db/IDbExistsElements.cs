using System.Threading.Tasks;
using trifenix.connect.interfaces.db;

namespace trifenix.connect.agro.interfaces.db
{
    public interface IDbExistsElements : IExistElement
    {
        Task<bool> ExistsDosesFromOrder(string idDoses);

        Task<bool> ExistsDosesExecutionOrder(string idDoses);

    }
}
