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
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.enums.searchModel;
using trifenix.agro.enums.input;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : MainReadOperation<Season>, IGenericOperation<Season, SeasonInput>
    {
        public SeasonOperations(IMainGenericDb<Season> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Season> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(SeasonInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var current = !input.Current.HasValue || input.Current.Value;


            var season = new Season
            {
                Id = id,
                Current = current,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                IdCostCenter = input.IdCostCenter
            };


            var validaCostCenter = await existElement.ExistsById<CostCenter>(input.IdCostCenter);

            if (!validaCostCenter) throw new Exception(string.Format(ErrorMessages.NotValidId, "Centro de Costos"));

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var validaId = await existElement.ExistsById<Season>(input.Id);

                if (!validaId) throw new Exception("No existe la temporada a modificar");
            }


            await repo.CreateUpdate(season);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.SEASON,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = string.Format("{0:MM/dd/yyyy}", input.StartDate)
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = string.Format("{0:MM/dd/yyyy}", input.EndDate)
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.COSTCENTER,
                            EntityId = input.IdCostCenter
                        }
                    },
                    RelatedEnumValues = new RelatedEnumValue[]{ 
                        new RelatedEnumValue{ 
                            EnumerationIndex = (int)EnumerationRelated.SEASON_CURRENT,
                            Value = (int)(current?CurrentSeason.CURRENT:CurrentSeason.NOT_CURRENT)
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
