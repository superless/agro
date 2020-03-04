using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class CostCenterOperations : MainReadOperationName<CostCenter, CostCenterInput>, IGenericOperation<CostCenter, CostCenterInput>
    {
        public CostCenterOperations(IMainGenericDb<CostCenter> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(CostCenterInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var costCenter = new CostCenter
            {
                Id = id,
                Name = input.Name,
                IdBusinessName = input.IdBusinessName
            };

            var valida = await Validate(input);

            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, costCenter.CosmosEntityName));

            var existsCostCenter = await existElement.ExistsById<BusinessName>(input.IdBusinessName);
            if (!existsCostCenter) throw new Exception(string.Format(ErrorMessages.NotValidId, "Razón social"));


            await repo.CreateUpdate(costCenter);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.COSTCENTER,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.BUSINESSNAME,
                            EntityId = input.IdBusinessName
                        }
                    },
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
