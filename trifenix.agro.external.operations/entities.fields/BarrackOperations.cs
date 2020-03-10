using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.fields {

    public class BarrackOperations : MainReadOperationName<Barrack, BarrackInput>, IGenericOperation<Barrack, BarrackInput> {

        private readonly ICommonQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<Barrack> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
        }

        private async Task<string> ValidaBarrack(BarrackInput input) {
            string errors = string.Empty;
            var seasonExists = await existElement.ExistsById<Season>(input.SeasonId);
            if (!seasonExists)
                errors += "No existe la temporada\r\n";
            var plotLandExists = await existElement.ExistsById<PlotLand>(input.IdPlotLand);
            if (!plotLandExists)
                errors += "No existe parcela\r\n";
            var varietyExists = await existElement.ExistsById<Variety>(input.IdVariety);
            if (!varietyExists)
                errors += "No existe variedad\r\n";
            var rootStockExists = await existElement.ExistsById<Rootstock>(input.IdRootstock);
            if (!rootStockExists)
                errors += "No existe portainjerto\r\n";
            var existsPollinator = await existElement.ExistsById<Variety>(input.IdPollinator);
            if (!existsPollinator)
                errors += "No existe polinizador\r\n";
            return errors;
        }

        public async Task<ExtPostContainer<string>> Save(BarrackInput input) {
            var id = input.Id??Guid.NewGuid().ToString("N");
            var barrack = new Barrack {
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
            if (!valida)
                throw new Exception(string.Format(ErrorMessages.NotValid, barrack.CosmosEntityName));
            var validaBarrak = await ValidaBarrack(input);
            if (!string.IsNullOrEmpty(validaBarrak))
                throw new Exception(validaBarrak);
            await repo.CreateUpdate(barrack);
            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromVariety(input.IdVariety);
            var relatedEntities = new List<RelatedId> {
                new RelatedId { EntityIndex = (int)EntityRelated.PLOTLAND, EntityId = input.IdPlotLand },
                new RelatedId { EntityIndex = (int)EntityRelated.POLLINATOR, EntityId = input.IdPollinator },
                new RelatedId { EntityIndex = (int)EntityRelated.ROOTSTOCK, EntityId = input.IdRootstock },
                new RelatedId { EntityIndex = (int)EntityRelated.SEASON , EntityId = input.SeasonId },
                new RelatedId { EntityIndex = (int)EntityRelated.VARIETY, EntityId = input.IdVariety }
            };

            // Eliminar antes de agregar
            var barrackIndex = search.FilterElements<EntitySearch>($"EntityIndex eq {(int)EntityRelated.BARRACK} and Id eq '{id}'");
            if(barrackIndex.Any())
                foreach(string idGeo in barrackIndex.ElementAt(0).RelatedIds.AsEnumerable().Where(relatedId => relatedId.EntityIndex == (int)EntityRelated.GEOPOINT).Select(relatedId => relatedId.EntityId))
                    search.DeleteElements(search.FilterElements<EntitySearch>($"EntityIndex eq {(int)EntityRelated.GEOPOINT} and Id eq '{idGeo}'"));


            var query = $"EntityIndex eq {(int)EntityRelated.GEOPOINT} and RelatedIds/any(elementId: elementId/EntityIndex eq {(int)EntityRelated.BARRACK} and elementId/EntityId eq '{id}')";

            var elements = search.FilterElements<EntitySearch>(query);
            if (elements.Any())
            {
                search.DeleteElements(search.FilterElements<EntitySearch>(query));
            }


            

            if (input.GeographicalPoints != null && input.GeographicalPoints.Any()) {
                var keysGeo = new List<EntitySearch>();
                foreach (var geo in input.GeographicalPoints) {
                    keysGeo.Add(new EntitySearch {
                        Id = Guid.NewGuid().ToString("N"),
                        EntityIndex = (int)EntityRelated.GEOPOINT,
                        Created = DateTime.Now,
                        RelatedProperties = new Property[] {
                            new Property { PropertyIndex = (int)PropertyRelated.GENERIC_LATITUDE, Value = $"{geo.Latitude}" },
                            new Property { PropertyIndex = (int)PropertyRelated.GENERIC_LONGITUDE, Value = $"{geo.Longitude}" }
                        },
                        RelatedIds = new RelatedId[] { 
                            new RelatedId{ EntityIndex = (int)EntityRelated.BARRACK, EntityId = id
                            }
                        } 
                    });
                }
                search.AddElements(keysGeo);
                relatedEntities.AddRange(keysGeo.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.GEOPOINT, EntityId = s.Id }));
            }

            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.BARRACK,
                    Created = DateTime.Now,
                    RelatedProperties= new Property[]{ 
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = input.Name },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION, Value = specieAbbv },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NUMBER_OF_PLANTS, Value = $"{input.NumberOfPlants}" },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_PLANT_IN_YEAR, Value = $"{input.PlantingYear}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.GENERIC_HECTARES, Value = $"{input.Hectares}" }
                    },
                    RelatedIds = relatedEntities.ToArray()
                }  
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}