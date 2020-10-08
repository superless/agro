using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class JobOperations<T> : MainOperation<Job, JobInput,T>, IGenericOperation<Job, JobInput> {
        public JobOperations(IMainGenericDb<Job> repo, IAgroSearch<T> search, ICommonDbOperations<Job> commonDb, IValidatorAttributes<JobInput, Job> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Job job) {
            await repo.CreateUpdate(job);
            search.AddDocument(job);
            return new ExtPostContainer<string> {
                IdRelated = job.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(JobInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var job = new Job {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(job);
            await repo.CreateEntityContainer(job);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}