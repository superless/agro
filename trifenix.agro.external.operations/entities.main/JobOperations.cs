using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class JobOperations : IJobOperations
    {
        private readonly IJobRepository _repo;
        private readonly ICommonDbOperations<Job> _commonDb;
        public JobOperations(IJobRepository repo, ICommonDbOperations<Job> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<Job>>> GetJobs()
        {
            var queryTargets = _repo.GetJobs();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<Job>> SaveEditJob(string id, string name)
        {
            var element = await _repo.GetJob(id);
            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateJob,
                 $"No existe objetivo aplicación con id : {id}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewJob(string name)
        {
            return await OperationHelper.CreateElement(_commonDb,_repo.GetJobs(),
                async s => await _repo.CreateUpdateJob(new Job
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"Ya existe cargo con nombre {name} "
            );
        }
    }
}
