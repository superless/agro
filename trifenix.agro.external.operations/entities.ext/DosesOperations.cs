using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.ext {
    public class DosesOperations : MainOperation<Dose, DosesInput>, IGenericOperation<Dose, DosesInput> {

        private readonly ICounters Counters;

        public DosesOperations(IMainGenericDb<Dose> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Dose> commonDb, ICounters counters, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            Counters = counters;
        }

        public async Task Remove(string id) {
            //Ambas consultas son identicas. Duplicado!
            var existsInOrder = await existElement.ExistsDosesFromOrder(id);
            var existsInExecution = await existElement.ExistsDosesExecutionOrder(id);
            if (!existsInExecution && !existsInOrder) {
                await repo.DeleteEntity(id);
                var query = $"EntityIndex eq {(int)EntityRelated.DOSES} and EntityId eq '{id}')";
                search.DeleteElements<EntitySearch>(query);
                return;
            }
            var dose = (await Get(id)).Result;
            dose.Active = false;
            await repo.CreateUpdate(dose);
            search.AddElements(new List<EntitySearch> { GetEntitySearch(dose) });
        }
        
        private RelatedId[] GetIdsRelated(Dose dose) {
            var list = new List<RelatedId> {
                new RelatedId {
                    EntityId = dose.IdProduct,
                    EntityIndex = (int)EntityRelated.PRODUCT
                }
            };
            if (dose.IdsApplicationTarget != null && dose.IdsApplicationTarget.Any())
                list.AddRange(dose.IdsApplicationTarget.Select(idAppTarget => new RelatedId { 
                    EntityId = idAppTarget,
                    EntityIndex = (int)EntityRelated.TARGET
                }));
            if (dose.IdSpecies != null && dose.IdSpecies.Any())
                list.AddRange(dose.IdSpecies.Select(idSpecie => new RelatedId {
                    EntityId = idSpecie,
                    EntityIndex = (int)EntityRelated.SPECIE
                }));
            if (dose.IdVarieties != null && dose.IdVarieties.Any())
                list.AddRange(dose.IdVarieties.Select(idVariety => new RelatedId {
                    EntityId = idVariety,
                    EntityIndex = (int)EntityRelated.VARIETY
                }));
            return list.ToArray();
        }

        private EntitySearch GetEntitySearch(Dose dose) {
            var relatedIds = GetIdsRelated(dose).ToList();
            var query = $"EntityIndex eq {(int)EntityRelated.WAITINGHARVEST} and RelatedIds/any(elementId: elementId/EntityIndex eq {(int)EntityRelated.DOSES} and elementId/EntityId eq '{dose.Id}')";
            search.DeleteElements<EntitySearch>(query);
            if (dose.WaitingToHarvest != null && dose.WaitingToHarvest.Any())
                foreach (var waitingHarvest in dose.WaitingToHarvest) {
                    var idSearch = Guid.NewGuid().ToString("N");
                    relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.WAITINGHARVEST, EntityId = idSearch });
                    search.AddElements(new List<EntitySearch> {
                        new EntitySearch {
                             Id= idSearch,
                             EntityIndex=(int)EntityRelated.WAITINGHARVEST,
                             Created = DateTime.Now,
                             RelatedIds = new RelatedId[]{ 
                                 new RelatedId { EntityIndex = (int)EntityRelated.DOSES,  EntityId = dose.Id},
                                 new RelatedId { EntityIndex = (int)EntityRelated.CERTIFIED_ENTITY, EntityId = waitingHarvest.IdCertifiedEntity }
                             },
                             RelatedProperties = new Property[]{ 
                                new Property { PropertyIndex = (int)PropertyRelated.WAITINGHARVEST_DAYS, Value = $"{waitingHarvest.WaitingDays}" },
                                new Property { PropertyIndex = (int)PropertyRelated.WAITINGHARVEST_PPM, Value = $"{waitingHarvest.Ppm}" },
                             }
                        }
                    });
                }
            return new EntitySearch {
                Id = dose.Id,
                EntityIndex = (int)EntityRelated.DOSES,
                Created = DateTime.Now,
                RelatedIds = relatedIds.ToArray(),
                RelatedProperties = new Property[]{
                    new Property { PropertyIndex = (int)PropertyRelated.DOSES_HOURSENTRYBARRACK, Value = $"{dose.HoursToReEntryToBarrack}" },
                    new Property { PropertyIndex = (int)PropertyRelated.DOSES_DAYSINTERVAL, Value = $"{dose.ApplicationDaysInterval}" },
                    new Property { PropertyIndex = (int)PropertyRelated.DOSES_SEQUENCE, Value = $"{dose.NumberOfSequentialApplication}" },
                    new Property { PropertyIndex = (int)PropertyRelated.DOSES_WAITINGDAYSLABEL, Value = $"{dose.WaitingDaysLabel}" },
                    new Property { PropertyIndex = (int)PropertyRelated.DOSES_WETTINGRECOMMENDED, Value = $"{dose.WettingRecommendedByHectares}" },
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_COUNTER, Value = $"{dose.Correlative}" }
                },
                RelatedEnumValues = new RelatedEnumValue[]{
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.GENERIC_ACTIVE, Value = dose.Active?1:0 },
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.GENERIC_DEFAULT, Value = dose.Default?1:0 },
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.DOSES_DOSESAPPLICATEDTO, Value = (int)dose.DosesApplicatedTo },
                }
            };
        }

        public async Task<ExtPostContainer<string>> Save(Dose dose) {
            await repo.CreateUpdate(dose);
            var productSearch = search.FilterElements<EntitySearch>($"EntityIndex eq {(int)EntityRelated.PRODUCT} and Id eq {dose.IdProduct}").FirstOrDefault();
            if (!productSearch.RelatedIds.Any(relatedId => relatedId.EntityIndex == (int)EntityRelated.DOSES && relatedId.EntityId == dose.Id)) {
                var relatedIds = productSearch.RelatedIds.ToList();
                relatedIds.Add(new RelatedId {
                    EntityId = dose.Id,
                    EntityIndex = (int)EntityRelated.DOSES
                });
                productSearch.RelatedIds = relatedIds.ToArray();
                search.AddElements(new List<EntitySearch> {
                    productSearch
                });
            }
            search.AddElements(new List<EntitySearch> {
                GetEntitySearch(dose)
            });
            return new ExtPostContainer<string> {
                IdRelated = dose.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(DosesInput dosesInput, bool isBatch) {
            //await Validate(dosesInput);
            var id = !string.IsNullOrWhiteSpace(dosesInput.Id) ? dosesInput.Id : Guid.NewGuid().ToString("N");
            long counter;
            //TODO: Revisar
            if (string.IsNullOrWhiteSpace(dosesInput.Id)) {
                var prevCounter = await Counters.GetLastCounterDoses(dosesInput.IdProduct);
                counter = ++prevCounter;
            }
            else
                counter = await Counters.GetCorrelativeFromDoses(dosesInput.Id);
            var dose = new Dose {
                Id = id,
                Correlative = counter,
                LastModified = DateTime.Now,
                ApplicationDaysInterval = dosesInput.ApplicationDaysInterval,
                HoursToReEntryToBarrack = dosesInput.HoursToReEntryToBarrack,
                DosesApplicatedTo = dosesInput.DosesApplicatedTo,
                DosesQuantityMax = dosesInput.DosesQuantityMax,
                DosesQuantityMin = dosesInput.DosesQuantityMin,
                IdsApplicationTarget = dosesInput.IdsApplicationTarget,
                IdSpecies = dosesInput.IdSpecies,
                IdVarieties = dosesInput.IdVarieties,
                NumberOfSequentialApplication = dosesInput.NumberOfSequentialApplication,
                IdProduct = dosesInput.IdProduct,
                Active = dosesInput.Active,
                Default = dosesInput.Default,
                WaitingDaysLabel = dosesInput.WaitingDaysLabel,
                WaitingToHarvest = dosesInput.WaitingToHarvest == null || !dosesInput.WaitingToHarvest.Any() ? new List<WaitingHarvest>() : dosesInput.WaitingToHarvest.Select(WH_Input => new WaitingHarvest {
                    IdCertifiedEntity = WH_Input.IdCertifiedEntity,
                    WaitingDays = WH_Input.WaitingDays,
                    Ppm = WH_Input.Ppm
                }).ToList(),
                WettingRecommendedByHectares = dosesInput.WettingRecommendedByHectares
            };
            if (!isBatch)
                return await Save(dose);
            await repo.CreateEntityContainer(dose);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}