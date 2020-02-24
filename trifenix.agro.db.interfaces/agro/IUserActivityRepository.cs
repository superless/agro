using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IUserActivityRepository {

        Task<string> CreateUpdateUserActivity(UserActivity userActivity);

        Task<UserActivity> GetUserActivity(string id);

        IQueryable<UserActivity> GetUserActivities();




    }
}
