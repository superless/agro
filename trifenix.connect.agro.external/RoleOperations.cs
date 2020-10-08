using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class RoleOperations<T> : MainOperation<Role, RoleInput,T>, IGenericOperation<Role, RoleInput> {
        public RoleOperations(IMainGenericDb<Role> repo, IAgroSearch<T> search, ICommonDbOperations<Role> commonDb, IValidatorAttributes<RoleInput, Role> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Role role) {
            await repo.CreateUpdate(role);
            search.AddDocument(role);
            return new ExtPostContainer<string> {
                IdRelated = role.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(RoleInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var role = new Role {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(role);
            await repo.CreateEntityContainer(role);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}