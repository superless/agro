using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.external.interfaces;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.external.operations.res;
using trifenix.agro.enums;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : MainReadOperation<Season>, IGenericOperation<Season, SeasonInput>
    {
        public SeasonOperations(IMainGenericDb<Season> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(SeasonInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var current = !input.Current.HasValue ? true : input.Current.Value;


            var costCenter = new Season
            {
                Id = id,
                Current = current,
                Start = input.StartDate,
                End = input.EndDate,
                IdCostCenter = input.IdCostCenter
            };


            var validaCostCenter = await existElement.ExistsElement<Season>("IdCostCenter", input.IdCostCenter);

            if (!validaCostCenter) throw new Exception(string.Format(ErrorMessages.NotValidId, "Centro de Costos"));

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var validaId = await existElement.ExistsElement<Season>(input.Id);

                if (!validaId) throw new Exception("No existe la temporada a modificar");
            }


            await repo.CreateUpdate(costCenter);

            search.AddEntities(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    EntityName = costCenter.CosmosEntityName,
                    NumbersRelated = new NumberEntityRelated[]{ 
                        new NumberEntityRelated{ 
                            EntityIndex = (int)EnumerationRelated.SEASON_CURRENT,
                            Number = current?1:0
                        }
                    },
                    IdsRelated = new IdsRelated[]{ 
                        new IdsRelated{ 
                            EntityIndex = (int)EntityRelated.COSTCENTER,
                            EntityId = input.IdCostCenter
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
