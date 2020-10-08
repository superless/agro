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
    public class RootstockOperations<T> : MainOperation<Rootstock, RootstockInput,T>, IGenericOperation<Rootstock, RootstockInput> {
        public RootstockOperations(IMainGenericDb<Rootstock> repo, IAgroSearch<T> search, ICommonDbOperations<Rootstock> commonDb, IValidatorAttributes<RootstockInput, Rootstock> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Rootstock rootstock) {
            await repo.CreateUpdate(rootstock);
            search.AddDocument(rootstock);

            return new ExtPostContainer<string> {
                IdRelated = rootstock.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(RootstockInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var rootstock = new Rootstock {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(rootstock);
            await repo.CreateEntityContainer(rootstock);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}