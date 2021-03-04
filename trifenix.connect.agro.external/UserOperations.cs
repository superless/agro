using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones realizadas por el usuario,
    /// dentro de esta se puede almacenar nuevos usuarios
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserOperations<T> : MainOperation<UserApplicator, UserApplicatorInput,T>, IGenericOperation<UserApplicator, UserApplicatorInput> {

        private readonly IGraphApi graphApi;

        public UserOperations(IMainGenericDb<UserApplicator> repo, IAgroSearch<T> search, IGraphApi graphApi, IValidatorAttributes<UserApplicatorInput> validator, ILogger log) : base(repo, search, validator, log)
        {
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