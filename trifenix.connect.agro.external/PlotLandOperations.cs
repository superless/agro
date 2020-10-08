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
    public class PlotLandOperations<T> : MainOperation<PlotLand, PlotLandInput,T>, IGenericOperation<PlotLand, PlotLandInput> {
        public PlotLandOperations(IMainGenericDb<PlotLand> repo, IAgroSearch<T> search, ICommonDbOperations<PlotLand> commonDb, IValidatorAttributes<PlotLandInput, PlotLand> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(PlotLand plotLand) {
            await repo.CreateUpdate(plotLand);
            search.AddDocument(plotLand);

            return new ExtPostContainer<string> {
                IdRelated = plotLand.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PlotLandInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var plotLand = new PlotLand {
                Id = id,
                Name = input.Name,
                IdSector = input.IdSector
            };
            if (!isBatch)
                return await Save(plotLand);
            await repo.CreateEntityContainer(plotLand);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}