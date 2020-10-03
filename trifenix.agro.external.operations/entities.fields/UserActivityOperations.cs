using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;

using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.fields {

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