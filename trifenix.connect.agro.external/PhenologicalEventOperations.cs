using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class PhenologicalEventOperations<T> : MainOperation<PhenologicalEvent, PhenologicalEventInput,T>, IGenericOperation<PhenologicalEvent, PhenologicalEventInput> {
        public PhenologicalEventOperations(IMainGenericDb<PhenologicalEvent> repo, IAgroSearch<T> search, ICommonDbOperations<PhenologicalEvent> commonDb, IValidatorAttributes<PhenologicalEventInput, PhenologicalEvent> validator) : base(repo, search, commonDb, validator) { }

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