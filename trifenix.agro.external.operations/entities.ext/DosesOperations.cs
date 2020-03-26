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
        private readonly ICounters counters;
        private readonly ICommonQueries queries;

        public DosesOperations(IMainGenericDb<Doses> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Doses> commonDb, ICounters counters, ICommonQueries queries) : base(repo, existElement, search, commonDb)
        {
            this.counters = counters;
            this.queries = queries;
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
                    EntityIndex = (int)EntityRelated.SPECIE
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
            //waitingharvest

            
            return list.ToArray();
        }


        public async Task Remove(string id)
        {

            
            var existsInOrder = await existElement.ExistsDosesFromOrder(id);
            var existsInExecution = await existElement.ExistsDosesExecutionOrder(id);
            if (!existsInExecution && !existsInOrder)
            {
                await repo.DeleteEntity(id);
                
                return;
            }

            var doses = await Store.GetEntity(id);
            doses.Active = false;
            await Store.CreateUpdate(doses);

            var dosesSearch = await GetEntitySearch(doses);
            search.AddElements(new List<EntitySearch>
            {
                dosesSearch
            });
            




        }


        private async Task<EntitySearch> GetEntitySearch(Doses input) {

            var ids = GetIdsRelated(input).ToList();

            search.DeleteElementsWithRelatedElement(EntityRelated.WAITINGHARVEST, EntityRelated.DOSES, input.Id);


            if (input.WaitingToHarvest!=null && input.WaitingToHarvest.Any())
            {
                foreach (var waitingHarvest in input.WaitingToHarvest)
                {
                    var idSearch = Guid.NewGuid().ToString("N");
                    ids.Add(new RelatedId { EntityIndex = (int)EntityRelated.WAITINGHARVEST, EntityId = idSearch });
                    search.AddElements(new List<EntitySearch> {
                        new EntitySearch{
                             EntityIndex=(int)EntityRelated.WAITINGHARVEST,
                             Id= idSearch,
                             Created = DateTime.Now,
                             RelatedIds = new RelatedId[]{ 
                                 new RelatedId{ EntityIndex= (int)EntityRelated.DOSES,  EntityId= input.Id},
                                 new RelatedId{ EntityIndex=(int)EntityRelated.CERTIFIED_ENTITY, EntityId=waitingHarvest.IdCertifiedEntity }
                             },
                             RelatedProperties = new Property[]{ 
                                new Property{PropertyIndex=(int)PropertyRelated.WAITINGHARVEST_DAYS, Value = $"{waitingHarvest.WaitingDays}" },
                                new Property{PropertyIndex=(int)PropertyRelated.WAITINGHARVEST_PPM, Value = $"{waitingHarvest.Ppm}" },
                             }
                        }
                    });
                }
            }



            var productName = await queries.GetEntityName<Product>(input.IdProduct);

            return new EntitySearch
            {
                Id = input.Id,
                EntityIndex = (int)EntityRelated.DOSES,
                Created = DateTime.Now,
                RelatedIds = ids.ToArray(),
                RelatedProperties = new Property[]{
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_HOURSENTRYBARRACK, Value = $"{input.HoursToReEntryToBarrack}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_DAYSINTERVAL, Value = $"{input.ApplicationDaysInterval}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_SEQUENCE, Value = $"{input.NumberOfSequentialApplication}" },                        
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_WETTINGRECOMMENDED, Value = $"{input.WettingRecommendedByHectares}" },
                        new Property{ PropertyIndex = (int)PropertyRelated.DOSES_WAITINGDAYSLABEL, Value = $"{input.WaitingDaysLabel}" },
                        new Property{  PropertyIndex = (int)PropertyRelated.GENERIC_COUNTER, Value = $"{input.Correlative}"},
                        new Property { PropertyIndex = (int)PropertyRelated.DOSES_MAX, Value = $"{input.DosesQuantityMax}" },
                        new Property { PropertyIndex = (int)PropertyRelated.DOSES_MIN, Value = $"{input.DosesQuantityMin}" }, 
                        new Property { PropertyIndex = (int)PropertyRelated.DOSES_MIN, Value = $"{input.DosesQuantityMin}" },
                        new Property { PropertyIndex = (int)PropertyRelated.PRODUCT_NAME, Value =productName },


                    },
                RelatedEnumValues = new RelatedEnumValue[] {
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

            long counter;
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var prevCounter = await counters.GetLastCounterDoses(input.IdProduct);
                counter = prevCounter+=1;
            }
            else {
                counter = await counters.GetCorrelativeFromDoses(input.Id);
            }


            var doses = new Doses
            {
                Id = id,
                Correlative = counter,
                LastModified = DateTime.Now,
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
                    WaitingDays = w.WaitingDays,
                    Ppm = w.Ppm
                
                }).ToList(),
                WettingRecommendedByHectares = input.WettingRecommendedByHectares
            };
            await repo.CreateUpdate(doses);
            var dosesSearch = await GetEntitySearch(doses);
            search.AddElements(new List<EntitySearch>
            {
                dosesSearch
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
