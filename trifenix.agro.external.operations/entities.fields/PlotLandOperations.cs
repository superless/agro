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

namespace trifenix.agro.external.operations.entities.fields
{
    public class PlotLandOperations : MainOperation<PlotLand, PlotLandInput>, IGenericOperation<PlotLand, PlotLandInput> {
        public PlotLandOperations(IMainGenericDb<PlotLand> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PlotLand> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

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