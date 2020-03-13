using System;
using System.Collections.Generic;
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

namespace trifenix.agro.external.operations.entities.fields
{
    public class PlotLandOperations : MainReadOperationName<PlotLand, PlotLandInput>, IGenericOperation<PlotLand, PlotLandInput>
    {
        public PlotLandOperations(IMainGenericDb<PlotLand> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<PlotLand> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(PlotLandInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var plotLand = new PlotLand
            {
                Id = id,
                Name = input.Name,
                IdSector = input.IdSector
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, plotLand.CosmosEntityName));

            var existsSector = await existElement.ExistsById<Sector>(input.IdSector);
            if (!existsSector) throw new Exception(string.Format(ErrorMessages.NotValidId, "Sector"));


            await repo.CreateUpdate(plotLand);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.PLOTLAND,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.SECTOR,
                            EntityId = input.IdSector
                        }
                    }
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
