using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
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

namespace trifenix.agro.external.operations.entities.main {
    public class PhenologicalEventOperations : MainOperation<PhenologicalEvent, PhenologicalEventInput>, IGenericOperation<PhenologicalEvent, PhenologicalEventInput> {
        public PhenologicalEventOperations(IMainGenericDb<PhenologicalEvent> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PhenologicalEvent> commonDb) : base(repo, existElement, search, commonDb) { }

        public override async Task Validate(PhenologicalEventInput phenologicalEventInput, bool isBatch) {
            await base.Validate(phenologicalEventInput, isBatch);
            List<string> errors = new List<string>(); 
            if (phenologicalEventInput.EndDate < phenologicalEventInput.StartDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        public async Task<ExtPostContainer<string>> Save(PhenologicalEvent phenologicalEvent) {
            await repo.CreateUpdate(phenologicalEvent);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = phenologicalEvent.Id,
                    EntityIndex = (int)EntityRelated.PHENOLOGICAL_EVENT,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = phenologicalEvent.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = string.Format("{0:dd/MM/yyyy}", phenologicalEvent.StartDate)
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = string.Format("{0:dd/MM/yyyy}", phenologicalEvent.EndDate)
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = phenologicalEvent.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PhenologicalEventInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var phenologicalEvent = new PhenologicalEvent {
                Id = id,
                Name = input.Name,
                StartDate = input.StartDate,
                EndDate = input.EndDate
            };
            if (!isBatch)
                return await Save(phenologicalEvent);
            await repo.CreateEntityContainer(phenologicalEvent);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}