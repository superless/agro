using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {

    public class TractorOperations : MainReadOperation<Tractor>, IGenericOperation<Tractor, TractorInput> {

        public TractorOperations(IMainGenericDb<Tractor> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Tractor> commonDb) : base(repo, existElement, search, commonDb) { }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(TractorInput input) {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            if (!string.IsNullOrWhiteSpace(input.Id)) {
                var existsId = await existElement.ExistsById<Tractor>(input.Id);
                if (!existsId)
                    throw new Exception(string.Format(ErrorMessages.NotValidId, "Tractor"));
                var existsCode = await existElement.ExistsWithPropertyValue<Tractor>("Code", input.Code, input.Id);
                if (existsCode)
                    throw new Exception("El código ya existe en otro tractor");
            }
            else {
                var existsCode = await existElement.ExistsWithPropertyValue<Tractor>("Code", input.Code);
                if (existsCode)
                    throw new Exception("El código ya existe en otro tractor");
            }
            var tractor = new Tractor {
                Id = id,
                Brand = input.Brand,
                Code = input.Code
            };
            await repo.CreateUpdate(tractor);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = id,
                    EntityIndex = (int)EntityRelated.TRACTOR,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = input.Brand },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_CODE, Value = input.Code }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}