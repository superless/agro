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

    public class TractorOperations<T> : MainOperation<Tractor, TractorInput,T>, IGenericOperation<Tractor, TractorInput> {

        public TractorOperations(IMainGenericDb<Tractor> repo,  IAgroSearch<T> search, ICommonDbOperations<Tractor> commonDb, IValidatorAttributes<TractorInput, Tractor> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Tractor tractor) {
            await repo.CreateUpdate(tractor);
            search.AddDocument(tractor);
            return new ExtPostContainer<string> {
                IdRelated = tractor.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(TractorInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var tractor = new Tractor {
                Id = id,
                Brand = input.Brand,
                Code = input.Code
            };
            if (!isBatch)
                return await Save(tractor);
            await repo.CreateEntityContainer(tractor);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}