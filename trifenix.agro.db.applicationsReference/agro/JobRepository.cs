using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class JobRepository : IJobRepository
    {

        private readonly IMainDb<Job> _db;
        public JobRepository(IMainDb<Job> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateJob(Job job)
        {
            return await _db.CreateUpdate(job);
        }

        public async Task<Job> GetJob(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Job> GetJobs()
        {
            return _db.GetEntities();
        }
    }
}
