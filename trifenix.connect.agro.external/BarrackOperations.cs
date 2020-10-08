using Microsoft.Azure.Documents.Spatial;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{

    public class BarrackOperations<T> : MainOperation<Barrack,BarrackInput,T>, IGenericOperation<Barrack, BarrackInput> {

        private readonly ICommonQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo,  IAgroSearch<T> search, ICommonQueries commonQueries, ICommonDbOperations<Barrack> commonDb, IValidatorAttributes<BarrackInput, Barrack> validator) : base(repo, search, commonDb, validator) {
            this.commonQueries = commonQueries;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Barrack barrack) {
            await repo.CreateUpdate(barrack);
            search.DeleteElementsWithRelatedElement(EntityRelated.GEOPOINT, EntityRelated.BARRACK, barrack.Id);
            search.AddDocument(barrack);
            return new ExtPostContainer<string> {
                IdRelated = barrack.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(BarrackInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var barrack = new Barrack {
                Id = id,
                Name = input.Name,
                Hectares = input.Hectares,
                IdPlotLand = input.IdPlotLand,
                IdPollinator = input.IdPollinator,
                IdRootstock = input.IdRootstock,
                IdVariety = input.IdVariety,
                NumberOfPlants = input.NumberOfPlants,
                PlantingYear = input.PlantingYear,
                SeasonId = input.SeasonId
            };
            if (input.GeographicalPoints != null && input.GeographicalPoints.Any())
                barrack.GeographicalPoints = input.GeographicalPoints.Select(geoPoint => new Point(geoPoint.Longitude, geoPoint.Latitude)).ToArray();
            if (!isBatch)
                return await Save(barrack);
            await repo.CreateEntityContainer(barrack);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}