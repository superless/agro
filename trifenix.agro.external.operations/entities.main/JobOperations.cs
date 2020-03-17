using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class JobOperations : MainOperation<Job, JobInput>, IGenericOperation<Job, JobInput> {
        public JobOperations(IMainGenericDb<Job> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Job> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(Job job) {
            await repo.CreateUpdate(job, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = job.Id,
                    EntityIndex = (int)EntityRelated.JOB,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = job.Name
                        }
                    },
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = job.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(JobInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var job = new Job {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(job);
            await repo.CreateUpdate(job, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}