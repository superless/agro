using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones de la actividad de los usuarios
    /// </summary>
    /// <typeparam name="T">Tipo de operacion a realizar</typeparam>
    public class UserActivityOperations<T> : MainOperation<UserActivity, UserActivityInput, T>, IGenericOperation<UserActivity, UserActivityInput> {

        private readonly string UserId;

        public UserActivityOperations(IMainGenericDb<UserActivity> repo, IAgroSearch<T> search, string userId, IValidatorAttributes<UserActivityInput> validator) : base(repo, search, validator) {
            UserId = userId;
        }

        

       

        public override async Task<ExtPostContainer<string>> SaveInput(UserActivityInput input) {
            await Validate(input);
            var id = Guid.NewGuid().ToString("N");
            var UserActivity = new UserActivity {
                Id = id,
                UserId = UserId,
                Action = input.Action,
                Date = input.Date,
                EntityName = input.EntityName,
                EntityId = input.EntityId,
            };
            await SaveDb(UserActivity);
            return await SaveSearch(UserActivity);
        }

        
    }

}