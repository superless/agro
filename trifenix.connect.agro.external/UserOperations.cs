using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{

    public class UserOperations<T> : MainOperation<UserApplicator, UserApplicatorInput,T>, IGenericOperation<UserApplicator, UserApplicatorInput> {

        private readonly IGraphApi graphApi;

        public UserOperations(IMainGenericDb<UserApplicator> repo, IAgroSearch<T> search, IGraphApi graphApi, ICommonDbOperations<UserApplicator> commonDb, IValidatorAttributes<UserApplicatorInput> validator) : base(repo, search, commonDb, validator) {
            this.graphApi = graphApi;
        }

      
        public override async Task<ExtPostContainer<string>> SaveInput(UserApplicatorInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var user = new UserApplicator {
                Id = id,
                Name = input.Name,
                Rut = input.Rut,
                Email = input.Email,
                IdsRoles = input.IdsRoles,
                IdJob = input.IdJob,
                IdNebulizer = input.IdNebulizer,
                IdTractor = input.IdTractor
            };
            if (string.IsNullOrWhiteSpace(input.Id))
                user.ObjectIdAAD = await graphApi.CreateUserIntoActiveDirectory(input.Name, input.Email);
            else
                user.ObjectIdAAD = (await Get(id)).Result.ObjectIdAAD;
            await SaveDb(user);
            return await SaveSearch(user);
        }

        
    }

}