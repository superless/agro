using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class CostCenterOperations : MainOperation<CostCenter, CostCenterInput>, IGenericOperation<CostCenter, CostCenterInput> {
        public CostCenterOperations(IMainGenericDb<CostCenter> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<CostCenter> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(CostCenter costCenter) {
            await repo.CreateUpdate(costCenter, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = costCenter.Id,
                    EntityIndex = (int)EntityRelated.COSTCENTER,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = costCenter.Name
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.BUSINESSNAME,
                            EntityId = costCenter.IdBusinessName
                        }
                    },
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = costCenter.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(CostCenterInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var costCenter = new CostCenter {
                Id = id,
                Name = input.Name,
                IdBusinessName = input.IdBusinessName
            };
            if (!isBatch)
                return await Save(costCenter);
            await repo.CreateUpdate(costCenter, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}