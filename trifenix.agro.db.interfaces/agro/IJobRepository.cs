using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IJobRepository
    {
        Task<string> CreateUpdateJob(Job job);

        Task<Job> GetJob(string id);

        IQueryable<Job> GetJobs();
    }
}
