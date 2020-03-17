using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class NebulizerOperations : MainOperation<Nebulizer, NebulizerInput>, IGenericOperation<Nebulizer, NebulizerInput> {
        public NebulizerOperations(IMainGenericDb<Nebulizer> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Nebulizer> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(Nebulizer nebulizer) {
            await repo.CreateUpdate(nebulizer, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = nebulizer.Id,
                    EntityIndex = (int)EntityRelated.NEBULIZER,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_BRAND,
                            Value = nebulizer.Brand
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_CODE,
                            Value = nebulizer.Code
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = nebulizer.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(NebulizerInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var nebulizer = new Nebulizer {
                Id = id,
                Brand = input.Brand,
                Code = input.Code
            };
            if (!isBatch)
                return await Save(nebulizer);
            await repo.CreateUpdate(nebulizer, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}