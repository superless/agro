using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{
    public interface ITimeStampDbQueries {
        Task<long[]> GetTimestamps<T>() where T:DocumentBase;
    }
}
