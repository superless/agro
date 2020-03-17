using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class PhenologicalEventOperations : MainOperation<PhenologicalEvent, PhenologicalEventInput>, IGenericOperation<PhenologicalEvent, PhenologicalEventInput> {
        public PhenologicalEventOperations(IMainGenericDb<PhenologicalEvent> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PhenologicalEvent> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> SaveInput(PhenologicalEventInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var phenologicalEvent = new PhenologicalEvent {
                Id = id,
                Name = input.Name,
                InitDate = input.StartDate,
                EndDate = input.EndDate
            };
            //if (input.EndDate < input.StartDate) throw new Exception(ErrorMessages.InitDateisOlderThenEndDate);
            await repo.CreateUpdate(phenologicalEvent, isBatch);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.PHENOLOGICAL_EVENT,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = string.Format("{0:dd/MM/yyyy}", input.StartDate)
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = string.Format("{0:dd/MM/yyyy}", input.EndDate)
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}