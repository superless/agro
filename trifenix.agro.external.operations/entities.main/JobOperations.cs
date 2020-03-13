using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class JobOperations : MainReadOperationName<Job, JobInput>, IGenericOperation<Job, JobInput>
    {
        public JobOperations(IMainGenericDb<Job> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Job> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(JobInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var job = new Job
            {
                Id = id,
                Name = input.Name
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, job.CosmosEntityName));

            await repo.CreateUpdate(job);

            var jobSearch = new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.JOB,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
                    },
                }
            };

            search.AddElements(jobSearch);


            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }
}
