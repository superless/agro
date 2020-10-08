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

    public class SectorOperations<T> : MainOperation<Sector, SectorInput,T>, IGenericOperation<Sector, SectorInput> {
        public SectorOperations(IMainGenericDb<Sector> repo, IAgroSearch<T> search, ICommonDbOperations<Sector> commonDb, IValidatorAttributes<SectorInput, Sector> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Sector sector) {
            await repo.CreateUpdate(sector);
            search.AddDocument(sector);
            return new ExtPostContainer<string> {
                IdRelated = sector.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SectorInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var sector = new Sector {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(sector);
            await repo.CreateEntityContainer(sector);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }
   
    }

}