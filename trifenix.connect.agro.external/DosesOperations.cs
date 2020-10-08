using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.search.model;
using trifenix.connect.util;

namespace trifenix.agro.external.operations.entities.ext
{
    public class DosesOperations<T> : MainOperation<Dose, DosesInput,T>, IGenericOperation<Dose, DosesInput> {
        private readonly IDbExistsElements existsElement;
        private readonly ICommonQueries Queries;

        public DosesOperations(IDbExistsElements existsElement, IMainGenericDb<Dose> repo,  IAgroSearch<T> search, ICommonDbOperations<Dose> commonDb, ICommonQueries queries, IValidatorAttributes<DosesInput, Dose> validator) : base(repo, search, commonDb, validator) {
            this.existsElement = existsElement;
            Queries = queries;
        }

        public async Task Remove(string id) {
            //Ambas consultas son identicas. Duplicado!
            var existsInOrder = await existsElement.ExistsDosesFromOrder(id);
            var existsInExecution = await existsElement.ExistsDosesExecutionOrder(id);
            if (!existsInExecution && !existsInOrder) {
                await repo.DeleteEntity(id);
                var query = $"index eq {(int)EntityRelated.DOSES} and id eq '{id}'";
                search.DeleteElements(query);
                return;
            }
            var dose = (await Get(id)).Result;
            dose.Active = false;
            await repo.CreateUpdate(dose);
            search.AddElements(search.GetEntitySearch(dose).ToList());
        }

        public async Task<ExtPostContainer<string>> Save(Dose dose) {
            await repo.CreateUpdate(dose);
            var productSearch = search.GetEntity(EntityRelated.PRODUCT, dose.IdProduct);

            if (!productSearch.rel.Any(relatedId => relatedId.index == (int)EntityRelated.DOSES && relatedId.id == dose.Id)) {
                productSearch.rel = productSearch.rel.Add(new RelatedId { id = dose.Id, index = (int)EntityRelated.DOSES });
                search.AddElement(productSearch);
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