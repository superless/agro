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

    public class UserActivityOperations<T> : MainOperation<UserActivity, UserActivityInput, T>, IGenericOperation<UserActivity, UserActivityInput> {

        private readonly string UserId;

        public UserActivityOperations(IMainGenericDb<UserActivity> repo, IAgroSearch<T> search, ICommonDbOperations<UserActivity> commonDb, string userId, IValidatorAttributes<UserActivityInput, UserActivity> validator) : base(repo, search, commonDb, validator) {
            UserId = userId;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(UserActivity userActivity) {
            await repo.CreateUpdate(userActivity);
            return new ExtPostContainer<string> {
                IdRelated = userActivity.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(UserActivityInput input, bool isBatch) {
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
            return await Save(UserActivity);
        }

    }

}