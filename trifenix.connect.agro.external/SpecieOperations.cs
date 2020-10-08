using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
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