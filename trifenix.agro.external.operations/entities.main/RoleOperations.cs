using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class RoleOperations : MainOperation<Role, RoleInput>, IGenericOperation<Role, RoleInput> {
        public RoleOperations(IMainGenericDb<Role> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Role> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> SaveInput(RoleInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var role = new Role {
                Id = id,
                Name = input.Name
            };
            await repo.CreateUpdate(role,isBatch);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.ROLE,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
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