using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.main {
    public class JobOperations : MainOperation<Job, JobInput>, IGenericOperation<Job, JobInput> {
        public JobOperations(IMainGenericDb<Job> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Job> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Job job) {
            await repo.CreateUpdate(job);
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