using Microsoft.Azure.Documents.Spatial;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones de los cuarteles, dentro valida y almacena 
    /// los datos ingresados
    /// </summary>
    /// <typeparam name="T">Las operaciones a ejecutar</typeparam>
    public class BarrackOperations<T> : MainOperation<Barrack,BarrackInput,T>, IGenericOperation<Barrack, BarrackInput> {

        private readonly ICommonAgroQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo,  IAgroSearch<T> search, ICommonAgroQueries commonQueries, ICommonDbOperations<Barrack> commonDb, IValidatorAttributes<BarrackInput> validator) : base(repo, search, commonDb, validator) {
            this.commonQueries = commonQueries;
        }

        public override async Task<ExtPostContainer<string>> SaveInput(BarrackInput input) {
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
            //GeoBarracks, dependencia de geospacial
            #if !CONNECT
            if (input.GeographicalPoints != null && input.GeographicalPoints.Any())
                barrack.GeographicalPoints = input.GeographicalPoints.Select(geoPoint => new Point(geoPoint.Lng, geoPoint.Lat)).ToArray();
            #endif

            search.DeleteElementsWithRelatedElement(EntityRelated.GEOPOINT, EntityRelated.BARRACK, barrack.Id);


            await SaveDb(barrack);
            return await SaveSearch(barrack);
        }

       
    }

}