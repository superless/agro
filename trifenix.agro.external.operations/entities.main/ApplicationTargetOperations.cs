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

namespace trifenix.agro.external.operations.entities.main {

    public class ApplicationTargetOperations : MainOperation<ApplicationTarget, ApplicationTargetInput>, IGenericOperation<ApplicationTarget, ApplicationTargetInput> {
        public ApplicationTargetOperations(IMainGenericDb<ApplicationTarget> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<ApplicationTarget> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(ApplicationTarget applicationTarget) {
            await repo.CreateUpdate(applicationTarget, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = applicationTarget.Id,
                    EntityIndex = (int)EntityRelated.TARGET,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = applicationTarget.Name
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = applicationTarget.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ApplicationTargetInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var target = new ApplicationTarget {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(target);
            await repo.CreateUpdate(target, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}