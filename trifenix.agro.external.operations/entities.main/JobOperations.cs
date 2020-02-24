using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
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
        public JobOperations(IMainGenericDb<Job> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
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

            search.AddSimpleEntities(new List<SimpleSearch>
            {
                new SimpleSearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Name,
                    EntityName = job.CosmosEntityName
                }
            });


            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }
}
