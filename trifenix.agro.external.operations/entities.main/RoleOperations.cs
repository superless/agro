using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class RoleOperations : MainReadOperation<Role>, IGenericOperation<Role, RoleInput>
    {
        private readonly IAgroSearch search;

        public RoleOperations(IMainGenericDb<Role> repo, IAgroSearch search) : base(repo)
        {
            this.search = search;
        }

        public async Task<ExtPostContainer<string>> Save(RoleInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var role = new Role
            {
                Id = id,
                Name = input.Name
            };
            await repo.CreateUpdate(role);

            search.AddSimpleEntities(new List<SimpleSearch>
            {
                new SimpleSearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Name,
                    EntityName = role.CosmosEntityName
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
