using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.operations.entities.fields {

    public class UserActivityOperations : MainFullReadOperation<UserActivity>, IGenericFullReadOperation<UserActivity, UserActivityInput> {

        private readonly IGraphApi graphApi;

        public UserActivityOperations(IMainDb<UserActivity> repo, ICommonDbOperations<UserActivity> commonDb, IGraphApi graphApi) : base(repo, commonDb) {
            this.graphApi = graphApi;
        }

        public async Task<ExtPostContainer<string>> Save(UserActivityInput input) {
            var id = Guid.NewGuid().ToString("N");
            var userId = await graphApi.GetUserIdFromToken();
            await repo.CreateUpdate(new UserActivity {
                Id = id,
                UserId = userId,
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