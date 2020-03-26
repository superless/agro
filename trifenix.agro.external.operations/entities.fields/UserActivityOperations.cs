using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations.entities.fields {

    public class UserActivityOperations : MainReadOperation<UserActivity>, IGenericOperation<UserActivity, UserActivityInput> {

        private readonly string UserId;

        public UserActivityOperations(IMainGenericDb<UserActivity> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<UserActivity> commonDb, string userId) : base(repo, existElement, search, commonDb) {
            UserId = userId;
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(UserActivityInput input) {
            var id = Guid.NewGuid().ToString("N");
            await repo.CreateUpdate(new UserActivity {
                Id = id,
                UserId = UserId,
                Action = input.Action,
                Date = input.Date,
                EntityName = input.EntityName,
                EntityId = input.EntityId,
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}