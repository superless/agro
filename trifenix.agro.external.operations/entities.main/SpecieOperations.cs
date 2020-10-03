using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.main
{
    public class SpecieOperations<T> : MainOperation<Specie, SpecieInput, T>, IGenericOperation<Specie, SpecieInput> {
        public SpecieOperations(IMainGenericDb<Specie> repo, IAgroSearch<T> search, ICommonDbOperations<Specie> commonDb, IValidatorAttributes<SpecieInput, Specie> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Specie specie) {
            await repo.CreateUpdate(specie);
            search.AddDocument(specie);
            return new ExtPostContainer<string> {
                IdRelated = specie.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SpecieInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var specie = new Specie {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(specie);
            await repo.CreateEntityContainer(specie);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}