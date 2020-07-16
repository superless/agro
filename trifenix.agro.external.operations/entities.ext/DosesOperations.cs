using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.local;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.util;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.az_search;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.ext
{
    public class DosesOperations : MainOperation<Dose, DosesInput>, IGenericOperation<Dose, DosesInput> {
    
        
        private readonly ICommonQueries Queries;

        public DosesOperations(IMainGenericDb<Dose> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Dose> commonDb, ICommonQueries queries, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            
            Queries = queries;
        }

        public async Task Remove(string id) {
            //Ambas consultas son identicas. Duplicado!
            var existsInOrder = await existElement.ExistsDosesFromOrder(id);
            var existsInExecution = await existElement.ExistsDosesExecutionOrder(id);
            if (!existsInExecution && !existsInOrder) {
                await repo.DeleteEntity(id);
                var query = $"EntityIndex eq {(int)EntityRelated.DOSES} and EntityId eq '{id}')";
                search.DeleteElements(query);
                return;
            }
            var dose = (await Get(id)).Result;
            dose.Active = false;
            await repo.CreateUpdate(dose);
            search.AddElements(new List<EntitySearch> { search.GetEntitySearch(dose) });
        }

        public async Task<ExtPostContainer<string>> Save(Dose dose) {
            await repo.CreateUpdate(dose);
            var productSearch = search.GetEntity(EntityRelated.PRODUCT, dose.IdProduct);

            if (!productSearch.rel.Any(relatedId => relatedId.index == (int)EntityRelated.DOSES && relatedId.id == dose.Id)) {
                productSearch.rel = productSearch.rel.Add(new RelatedId { id = dose.Id, index = (int)EntityRelated.DOSES });
                search.AddElements(new List<EntitySearch> { productSearch });
            }
            search.AddDocument(dose);

            return new ExtPostContainer<string> {
                IdRelated = dose.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(DosesInput dosesInput, bool isBatch) {
            //await Validate(dosesInput);
            var id = !string.IsNullOrWhiteSpace(dosesInput.Id) ? dosesInput.Id : Guid.NewGuid().ToString("N");


            
            var dose = new Dose {
                Id = id,
                
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