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

namespace trifenix.agro.external.operations.entities.ext
{
    public class DosesOperations : MainOperation<Dose, DosesInput>, IGenericOperation<Dose, DosesInput> {
        public DosesOperations(IMainGenericDb<Dose> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Dose> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<string> Validate(DosesInput input) {
            string errors = string.Empty;
            if (!string.IsNullOrWhiteSpace(input.Id)) {  //PUT
                var existsId = await existElement.ExistsById<Dose>(input.Id, false);
                if (!existsId)
                    errors += $"No existe dosis con id {input.Id}.";
            }
            var existProduct = await existElement.ExistsById<Product>(input.IdProduct, false);
            if (!existProduct)
                errors += $"No existe producto con id {input.IdProduct}";
            if (input.IdSpecies.Any()) {
                foreach (string idSpecie in input.IdSpecies.Distinct()) {
                    bool existSpecie = await existElement.ExistsById<Specie>(idSpecie, false);
                    if(!existSpecie)
                        errors += $"No existe la especie con id {idSpecie}.";
                }
            }
            if (input.IdVarieties.Any()) {
                foreach (string idVariety in input.IdVarieties.Distinct()) {
                    bool existVariety = await existElement.ExistsById<Variety>(idVariety, false);
                    if(!existVariety)
                        errors += $"No existe la variedad con id {idVariety}.";
                }
            }
            if (input.IdsApplicationTarget.Any()) {
                foreach (string idApplicationTarget in input.IdsApplicationTarget) {
                    bool existApplicationTarget = await existElement.ExistsById<ApplicationTarget>(idApplicationTarget, false);
                    if(!existApplicationTarget)
                        errors += $"No existe el objetivo de aplicacion con id {idApplicationTarget}.";
                }
            }
            if (input.WaitingToHarvest.Any()) {
                foreach (string idCertifiedEntity in input.WaitingToHarvest.Select(wth => wth.IdCertifiedEntity).Distinct()) {
                    bool existCertifiedEntity = await existElement.ExistsById<CertifiedEntity>(idCertifiedEntity, false);
                    if(!existCertifiedEntity)
                        errors += $"No existe la entidad certificadora con id {idCertifiedEntity}.";
                }
            }
            return errors.Replace(".",".\r\n");  
        }

        private RelatedId[] GetIdsRelated(Dose dose) {
            var list = new List<RelatedId> { new RelatedId { EntityId = dose.IdProduct, EntityIndex = (int)EntityRelated.PRODUCT } };
            if (dose.IdsApplicationTarget != null && dose.IdsApplicationTarget.Any()) {
                list.AddRange(dose.IdsApplicationTarget.Select(s => new RelatedId { 
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.TARGET
                }));
            }
            if (dose.IdSpecies != null && dose.IdSpecies.Any()) {
                list.AddRange(dose.IdSpecies.Select(s => new RelatedId {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.PREORDER
                }));
            }
            if (dose.IdVarieties != null && dose.IdVarieties.Any()) {
                list.AddRange(dose.IdVarieties.Select(s => new RelatedId {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.VARIETY
                }));
            }
            if (dose.WaitingToHarvest != null && dose.WaitingToHarvest.Any()) {
                list.AddRange(dose.WaitingToHarvest.Select(s=>s.IdCertifiedEntity).Select(s => new RelatedId {
                    EntityId = s,
                    EntityIndex = (int)EntityRelated.CERTIFIED_ENTITY
                }));
            }
            return list.ToArray();
        }

        public async Task<ExtPostContainer<string>> Save(Dose dose) {
            await repo.CreateUpdate(dose, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = dose.Id,
                    EntityIndex = (int)EntityRelated.DOSES,
                    Created = DateTime.Now,
                    RelatedIds = GetIdsRelated(dose),
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = dose.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(DosesInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var dose = new Dose {
                Id = id,
                ApplicationDaysInterval = input.ApplicationDaysInterval,
                DaysToReEntryToBarrack = input.DaysToReEntryToBarrack,
                DosesApplicatedTo = input.DosesApplicatedTo,
                DosesQuantityMax = input.DosesQuantityMax,
                DosesQuantityMin = input.DosesQuantityMin,
                IdsApplicationTarget = input.IdsApplicationTarget,
                IdSpecies = input.IdSpecies,
                IdVarieties = input.IdVarieties,
                NumberOfSequentialApplication = input.NumberOfSequentialApplication,
                IdProduct = input.IdProduct,
                WaitingDaysLabel = input.WaitingDaysLabel,
                WaitingToHarvest = input.WaitingToHarvest==null || !input.WaitingToHarvest.Any()?new List<WaitingHarvest>(): input.WaitingToHarvest.Select(w=>new WaitingHarvest { 
                    IdCertifiedEntity = w.IdCertifiedEntity,
                    WaitingDays = w.WaitingDays
                }).ToList(),
                WettingRecommendedByHectares = input.WettingRecommendedByHectares
            };
            if (!isBatch)
                return await Save(dose);
            await repo.CreateUpdate(dose, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}