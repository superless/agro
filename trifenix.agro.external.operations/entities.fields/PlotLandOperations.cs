using System;
using System.Collections.Generic;
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

namespace trifenix.agro.external.operations.entities.fields {
    public class PlotLandOperations : MainOperation<PlotLand, PlotLandInput>, IGenericOperation<PlotLand, PlotLandInput> {
        public PlotLandOperations(IMainGenericDb<PlotLand> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PlotLand> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(PlotLand plotLand) {
            await repo.CreateUpdate(plotLand, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = plotLand.Id,
                    EntityIndex = (int)EntityRelated.PLOTLAND,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = plotLand.Name
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.SECTOR,
                            EntityId = plotLand.IdSector
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = plotLand.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PlotLandInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var plotLand = new PlotLand {
                Id = id,
                Name = input.Name,
                IdSector = input.IdSector
            };
            if (!isBatch)
                return await Save(plotLand);
            await repo.CreateUpdate(plotLand, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}