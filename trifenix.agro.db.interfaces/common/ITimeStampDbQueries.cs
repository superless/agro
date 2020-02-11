using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.common {
    public interface ITimeStampDbQueries {
        Task<long[]> GetTimestamps<T>() where T:DocumentBase;
    }
}
