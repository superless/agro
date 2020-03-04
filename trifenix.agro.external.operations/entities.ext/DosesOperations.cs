using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
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
        public DosesOperations(IMainGenericDb<Doses> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        private RelatedId[] GetIdsRelated(DosesInput input)
        {
            var list = new List<RelatedId>();
            list.Add(new RelatedId
            {
                EntityId = input.IdProduct,
                EntityIndex = (int)EntityRelated.PRODUCT
            });

            if (input.idsApplicationTarget != null && input.idsApplicationTarget.Any())
            {
                list.AddRange(input.idsApplicationTarget.Select(s=>new RelatedId { 
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




            var doses = new Doses
            {
                Id = id,
                ApplicationDaysInterval = input.ApplicationDaysInterval,
                DaysToReEntryToBarrack = input.DaysToReEntryToBarrack,
                DosesApplicatedTo = input.DosesApplicatedTo,
                DosesQuantityMax = input.DosesQuantityMax,
                DosesQuantityMin = input.DosesQuantityMin,
                IdsApplicationTarget = input.idsApplicationTarget,
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
            await repo.CreateUpdate(doses);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.DOSES,
                    Created = DateTime.Now,
                    RelatedIds = GetIdsRelated(input),
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
