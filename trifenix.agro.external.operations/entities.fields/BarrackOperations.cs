using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.fields
{
    public class BarrackOperations : MainReadOperationName<Barrack, BarrackInput>, IGenericOperation<Barrack, BarrackInput>
    {
        private readonly ICommonQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
        }


        private async Task<string> ValidaBarrack(BarrackInput input) {

            var seasonExists = await existElement.ExistsElement<Season>(input.SeasonId);

            if (!seasonExists) return "No existe la temporada";

            var plotLandExists = await existElement.ExistsElement<PlotLand>(input.IdPlotLand);

            if (!plotLandExists) return "No existe parcela";

            var varietyExists = await existElement.ExistsElement<Variety>(input.IdVariety);

            if (!varietyExists) return "no existe variedad";

            var rootStockExists = await existElement.ExistsElement<Rootstock>(input.IdRootstock);

            if (!rootStockExists) return "no existe portainjerto";


            var existsPollinator = await existElement.ExistsElement<Variety>(input.IdPollinator);

            if (!existsPollinator) return "No existe polinizador";

            return string.Empty;

        }

        public async Task<ExtPostContainer<string>> Save(BarrackInput input)
        {

            
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var barrack = new Barrack
            {
                Id = id,
                Name = input.Name,
                GeographicalPoints = input.GeographicalPoints,
                Hectares = input.Hectares,
                IdPlotLand = input.IdPlotLand,
                IdPollinator = input.IdPollinator,
                IdRootstock = input.IdRootstock,
                IdVariety = input.IdVariety,
                NumberOfPlants = input.NumberOfPlants,
                PlantingYear = input.PlantingYear,
                SeasonId = input.SeasonId
            };



            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, barrack.CosmosEntityName));
            
            var validaBarrak = await ValidaBarrack(input);

            if (!string.IsNullOrWhiteSpace(validaBarrak)) throw new Exception(validaBarrak);



            await repo.CreateUpdate(barrack);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromVariety(input.IdVariety);

            var keysGeo = new List<EntitySearch>();
            var relatedEntities = new List<RelatedId>
            {
                new RelatedId{ EntityIndex = (int)EntityRelated.SEASON , EntityId = input.SeasonId },
                new RelatedId{ EntityIndex = (int)EntityRelated.PLOTLAND, EntityId = input.IdPlotLand },
                new RelatedId { EntityIndex = (int)EntityRelated.VARIETY, EntityId = input.IdVariety },
                new RelatedId { EntityIndex = (int)EntityRelated.POLLINATOR, EntityId = input.IdPollinator },
                new RelatedId { EntityIndex = (int)EntityRelated.ROOTSTOCK, EntityId = input.IdRootstock }
            };
            //TODO : ELiminar geoPoints
            if (input.GeographicalPoints != null && input.GeographicalPoints.Any())
            {
                foreach (var geo in input.GeographicalPoints)
                {

                    keysGeo.Add(new EntitySearch
                    {
                        Created = DateTime.Now,
                        EntityIndex = (int)EntityRelated.GEOPOINT,
                        Id = Guid.NewGuid().ToString("N"),
                        RelatedProperties = new Property[] {
                            new Property {
                                PropertyIndex = (int)PropertyRelated.GENERIC_LATITUDE,
                                Value = $"{geo.Latitude}"
                            },
                            new Property {
                                PropertyIndex = (int)PropertyRelated.GENERIC_LONGITUDE,
                                Value = $"{geo.Longitude}"
                            }
                        }

                    });
                }

                search.AddElements(keysGeo);
                relatedEntities.AddRange(keysGeo.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.GEOPOINT, EntityId = s.Id }));
            }

            
            

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    RelatedProperties= new Property[]{ 
                        new Property{ 
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_CODE,
                            Value = specieAbbv
                        },
                        new Property{ 
                            PropertyIndex = (int)PropertyRelated.GENERIC_NUMBER_OF_PLANTS,
                            Value = $"{input.NumberOfPlants}"
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_PLANT_IN_YEAR,
                            Value = $"{input.PlantingYear}"
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_HECTARES,
                            Value = $"{input.Hectares}"
                        }
                    },
                    EntityIndex = (int)EntityRelated.BARRACK,
                    RelatedIds = relatedEntities.ToArray()
                    }
                    
                  
                
            });


            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }


}
