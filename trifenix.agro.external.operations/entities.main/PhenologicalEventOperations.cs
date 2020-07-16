using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.main
{
    public class PhenologicalEventOperations : MainOperation<PhenologicalEvent, PhenologicalEventInput>, IGenericOperation<PhenologicalEvent, PhenologicalEventInput> {
        public PhenologicalEventOperations(IMainGenericDb<PhenologicalEvent> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PhenologicalEvent> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public override async Task Validate(PhenologicalEventInput phenologicalEventInput) {
            await base.Validate(phenologicalEventInput);
            List<string> errors = new List<string>(); 
            if (phenologicalEventInput.EndDate < phenologicalEventInput.StartDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        public async Task<ExtPostContainer<string>> Save(PhenologicalEvent phenologicalEvent) {
            await repo.CreateUpdate(phenologicalEvent);
            search.AddDocument(phenologicalEvent);
            return new ExtPostContainer<string> {
                IdRelated = phenologicalEvent.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PhenologicalEventInput input, bool isBatch) {
            await Validate(input);
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

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

    }

}