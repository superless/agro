using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
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
    public class VarietyOperations : MainOperation<Variety, VarietyInput>, IGenericOperation<Variety, VarietyInput> {
        public VarietyOperations(IMainGenericDb<Variety> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonDbOperations<Variety> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Variety variety) {
            await repo.CreateUpdate(variety);
            search.AddDocument(variety);
            return new ExtPostContainer<string> {
                IdRelated = variety.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(VarietyInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var variety = new Variety {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation,
                IdSpecie = input.IdSpecie
            };
            if (!isBatch)
                return await Save(variety);
            await repo.CreateEntityContainer(variety);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}