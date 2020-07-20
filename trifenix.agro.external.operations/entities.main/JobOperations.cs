using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.main
{
    public class JobOperations : MainOperation<Job, JobInput>, IGenericOperation<Job, JobInput> {
        public JobOperations(IMainGenericDb<Job> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonDbOperations<Job> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

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