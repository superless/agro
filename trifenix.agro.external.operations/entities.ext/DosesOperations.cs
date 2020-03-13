using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext
{
    public class DosesOperations : MainReadOperation<Doses>, IGenericOperation<Doses, DosesInput>
    {
        public DosesOperations(IMainGenericDb<Doses> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Doses> commonDb) : base(repo, existElement, search, commonDb)
        {
        }

        private RelatedId[] GetIdsRelated(Doses input)
        {
            var list = new List<RelatedId>
            {
                new RelatedId
                {
                    EntityId = input.IdProduct,
                    EntityIndex = (int)EntityRelated.PRODUCT
                }
            };

            if (input.IdsApplicationTarget != null && input.IdsApplicationTarget.Any())
            {
                list.AddRange(input.IdsApplicationTarget.Select(s=>new RelatedId { 
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.TARGET
                }));
            }

            if (input.IdSpecies != null && input.IdSpecies.Any())
            {
                list.AddRange(input.IdSpecies.Select(s => new RelatedId
                {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.PREORDER
                }));
            }

            if (input.IdVarieties != null && input.IdVarieties.Any())
            {
                list.AddRange(input.IdVarieties.Select(s => new RelatedId
                {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.VARIETY
                }));
            }

            if (input.WaitingToHarvest != null && input.WaitingToHarvest.Any())
            {
                list.AddRange(input.WaitingToHarvest.Select(s=>s.IdCertifiedEntity).Select(s => new RelatedId
                {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.CERTIFIED_ENTITY
                }));
            }
            return list.ToArray();
        }


        public async Task Remove(string id)
        {

            var queryOrder = $"SELECT value count(1) from c join dosesOrder in c.DosesOrder where dosesOrder.IdDoses = '{id}'";
            var existsInOrder = await existElement.ExistsCustom<ApplicationOrder>(queryOrder);
            var existsInExecution = await existElement.ExistsCustom<ExecutionOrder>(queryOrder);
            if (!existsInExecution && !existsInOrder)
            {
                await repo.DeleteEntity(id);
                
                return;
            }

            var doses = await Store.GetEntity(id);
            doses.Active = false;
            await Store.CreateUpdate(doses);

            search.AddElements(new List<EntitySearch>
            {
                GetEntitySearch(doses)
            });
            




        }


        private EntitySearch GetEntitySearch(Doses input) {
            return new EntitySearch
            {
                Id = input.Id,
                EntityIndex = (int)EntityRelated.DOSES,
                Created = DateTime.Now,
                RelatedIds = GetIdsRelated(input),
                RelatedProperties = new Property[]{
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_HOURSENTRYBARRACK, Value = $"{input.HoursToReEntryToBarrack}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_DAYSINTERVAL, Value = $"{input.ApplicationDaysInterval}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_SEQUENCE, Value = $"{input.NumberOfSequentialApplication}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_WAITINGDAYSLABEL, Value = $"{input.WaitingDaysLabel}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_WETTINGRECOMMENDED, Value = $"{input.WettingRecommendedByHectares}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_WAITINGDAYSLABEL, Value = $"{input.WaitingDaysLabel}" },
                    },
                RelatedEnumValues = new RelatedEnumValue[]{
                        new RelatedEnumValue{  EnumerationIndex = (int)EnumerationRelated.GENERIC_ACTIVE, Value = input.Active?1:0},
                        new RelatedEnumValue{  EnumerationIndex = (int)EnumerationRelated.GENERIC_DEFAULT, Value = input.Default?1:0},
                        new RelatedEnumValue{  EnumerationIndex = (int)EnumerationRelated.DOSES_DOSESAPPLICATEDTO, Value = (int)input.DosesApplicatedTo},
                    }
            };
        }


        public async Task<ExtPostContainer<string>> Save(DosesInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var validaDoses = DosesHelper.ValidaDoses(existElement, input);

            if (!string.IsNullOrWhiteSpace(validaDoses)) throw new Exception(validaDoses);

            var validaProduct = await existElement.ExistsById<Product>(input.IdProduct);

            if (!validaProduct) throw new Exception("el identificador de producto para dosis no se encuentra en la base de datos");

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var validaIdExists = await existElement.ExistsById<Doses>(input.Id);
                if (!validaIdExists) throw new Exception("No existe el id de la dosis a modificar");
            }

            var query = $"EntityIndex eq {(int)EntityRelated.WAITINGHARVEST} and RelatedIds/any(elementId: elementId/EntityIndex eq {(int)EntityRelated.DOSES} and elementId/EntityId eq '{id}')";

            var elements = search.FilterElements<EntitySearch>(query);
            if (elements.Any())
            {
                search.DeleteElements(search.FilterElements<EntitySearch>(query));
            }


            var doses = new Doses
            {
                Id = id,
                ApplicationDaysInterval = input.ApplicationDaysInterval,
                HoursToReEntryToBarrack = input.HoursToReEntryToBarrack,
                DosesApplicatedTo = input.DosesApplicatedTo,
                DosesQuantityMax = input.DosesQuantityMax,
                DosesQuantityMin = input.DosesQuantityMin,
                IdsApplicationTarget = input.idsApplicationTarget,
                IdSpecies = input.IdSpecies,
                IdVarieties = input.IdVarieties,
                NumberOfSequentialApplication = input.NumberOfSequentialApplication,
                IdProduct = input.IdProduct,
                Active = input.Active,
                Default = input.Default,
                WaitingDaysLabel = input.WaitingDaysLabel,
                WaitingToHarvest = input.WaitingToHarvest==null || !input.WaitingToHarvest.Any()?new List<WaitingHarvest>(): input.WaitingToHarvest.Select(w=>new WaitingHarvest { 
                    IdCertifiedEntity = w.IdCertifiedEntity,
                    WaitingDays = w.WaitingDays
                
                }).ToList(),
                WettingRecommendedByHectares = input.WettingRecommendedByHectares
            };
            await repo.CreateUpdate(doses);

            search.AddElements(new List<EntitySearch>
            {
                GetEntitySearch(doses)
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
