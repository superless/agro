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
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.fields {

    public class BarrackOperations : MainOperation<Barrack,BarrackInput>, IGenericOperation<Barrack, BarrackInput> {

        private readonly ICommonQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<Barrack> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.commonQueries = commonQueries;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Barrack barrack) {
            await repo.CreateUpdate(barrack);
            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromVariety(barrack.IdVariety);
            var relatedEntities = new List<RelatedId> {
                new RelatedId { EntityIndex = (int)EntityRelated.PLOTLAND, EntityId = barrack.IdPlotLand },
                new RelatedId { EntityIndex = (int)EntityRelated.SEASON , EntityId = barrack.SeasonId },
                new RelatedId { EntityIndex = (int)EntityRelated.VARIETY, EntityId = barrack.IdVariety }
            };
            if (!string.IsNullOrWhiteSpace(barrack.IdRootstock))
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.ROOTSTOCK, EntityId = barrack.IdRootstock });
            if (!string.IsNullOrWhiteSpace(barrack.IdPollinator))
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.POLLINATOR, EntityId = barrack.IdPollinator });
            // Eliminar antes de agregar
            search.DeleteElements<EntitySearch>($"EntityIndex eq {(int)EntityRelated.GEOPOINT} and RelatedIds/any(elementId: elementId/EntityIndex eq {(int)EntityRelated.BARRACK} and elementId/EntityId eq '{barrack.Id}')");
            if (barrack.GeographicalPoints != null && barrack.GeographicalPoints.Any()) {
                var keysGeo = new List<EntitySearch>();
                foreach (var geo in barrack.GeographicalPoints) {
                    keysGeo.Add(new EntitySearch {
                        Id = Guid.NewGuid().ToString("N"),
                        EntityIndex = (int)EntityRelated.GEOPOINT,
                        Created = DateTime.Now,
                        RelatedProperties = new Property[] {
                            new Property { PropertyIndex = (int)PropertyRelated.GENERIC_LATITUDE, Value = $"{geo.Latitude}" },
                            new Property { PropertyIndex = (int)PropertyRelated.GENERIC_LONGITUDE, Value = $"{geo.Longitude}" }
                        },
                        RelatedIds = new RelatedId[] {
                            new RelatedId { EntityIndex = (int)EntityRelated.BARRACK, EntityId = barrack.Id }
                        }
                    });
                }
                relatedEntities.AddRange(keysGeo.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.GEOPOINT, EntityId = s.Id }));
                search.AddElements(keysGeo);
            }
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = barrack.Id,
                    EntityIndex = (int)EntityRelated.BARRACK,
                    Created = DateTime.Now,
                    RelatedProperties= new Property[]{
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = barrack.Name },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION, Value = specieAbbv },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NUMBER_OF_PLANTS, Value = $"{barrack.NumberOfPlants}" },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_PLANT_IN_YEAR, Value = $"{barrack.PlantingYear}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.GENERIC_HECTARES, Value = $"{barrack.Hectares}" }
                    },
                    RelatedIds = relatedEntities.ToArray()
                }
            });
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