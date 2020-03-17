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
using trifenix.agro.enums;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : MainOperation<Season, SeasonInput>, IGenericOperation<Season, SeasonInput> {
        public SeasonOperations(IMainGenericDb<Season> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Season> commonDb) : base(repo, existElement, search, commonDb) { }

        public override async Task Validate(SeasonInput executionOrderStatusInput, bool isBatch) {
            await base.Validate(executionOrderStatusInput, isBatch);
        }

        public async Task<ExtPostContainer<string>> Save(Season season) {
            await repo.CreateUpdate(season, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = season.Id,
                    EntityIndex = (int)EntityRelated.SEASON,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = string.Format("{0:MM/dd/yyyy}", season.StartDate)
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = string.Format("{0:MM/dd/yyyy}", season.EndDate)
                        }
                    },
                    RelatedIds = new RelatedId[] {
                        new RelatedId {
                            EntityIndex = (int)EntityRelated.COSTCENTER,
                            EntityId = season.IdCostCenter
                        }
                    },
                    RelatedEnumValues = new RelatedEnumValue[] {
                        new RelatedEnumValue {
                            EnumerationIndex = (int)EnumerationRelated.SEASON_CURRENT,
                            Value = (int)(season.Current?CurrentSeason.CURRENT:CurrentSeason.NOT_CURRENT)
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = season.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SeasonInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var current = !input.Current.HasValue || input.Current.Value;
            var season = new Season {
                Id = id,
                Current = current,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                IdCostCenter = input.IdCostCenter
            };
            if (!isBatch)
                return await Save(season);
            await repo.CreateUpdate(season, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}