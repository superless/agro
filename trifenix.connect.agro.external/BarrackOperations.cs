using Microsoft.Azure.Documents.Spatial;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.exception;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones de los cuarteles, dentro valida y almacena 
    /// los datos ingresados
    /// </summary>
    /// <typeparam name="T">Las operaciones a ejecutar</typeparam>
    public class BarrackOperations<T> : MainOperation<Barrack,BarrackInput,T>, IGenericOperation<Barrack, BarrackInput> {

        private readonly ICommonAgroQueries Queries;

        public BarrackOperations(IMainGenericDb<Barrack> repo,  IAgroSearch<T> search, ICommonAgroQueries Queries, IValidatorAttributes<BarrackInput> validator, ILogger log) : base(repo, search, validator, log)
        {
            this.Queries = Queries;
        }

        public override async Task Validate(BarrackInput input)
        {
            await base.Validate(input);
            var season = await Queries.GetSeasonStatus(input.IdSeason);
            var seasonStatus = bool.Parse(season);
            if (!seasonStatus)
            {
                throw new CustomException("La temporada ingresada no se encuentra activa");
            }
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
                IdSeason = input.IdSeason
            };
            

            search.DeleteElementsWithRelatedElement(EntityRelated.GEOPOINT, EntityRelated.BARRACK, barrack.Id);


            await SaveDb(barrack);
            return await SaveSearch(barrack);
        }

       
    }

}