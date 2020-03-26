using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.main {

    public class TractorOperations : MainOperation<Tractor, TractorInput>, IGenericOperation<Tractor, TractorInput> {

        public TractorOperations(IMainGenericDb<Tractor> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Tractor> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Tractor tractor) {
            await repo.CreateUpdate(tractor);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = tractor.Id,
                    EntityIndex = (int)EntityRelated.TRACTOR,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = tractor.Brand },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_CODE, Value = tractor.Code }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = tractor.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(TractorInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var tractor = new Tractor {
                Id = id,
                Brand = input.Brand,
                Code = input.Code
            };
            if (!isBatch)
                return await Save(tractor);
            await repo.CreateEntityContainer(tractor);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}