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

namespace trifenix.agro.external.operations.entities.fields
{
    public class UserActivityOperations : MainFullReadOperation<UserActivity>, IGenericFullReadOperation<UserActivity, UserActivityInput>
    {
        private readonly IGraphApi graphApi;

        public UserActivityOperations(IMainDb<UserActivity> repo, ICommonDbOperations<UserActivity> commonDb, IGraphApi graphApi) : base(repo, commonDb)
        {
            this.graphApi = graphApi;
        }

        public async Task<ExtPostContainer<string>> Save(UserActivityInput input)
        {
            var id = input.Id ?? Guid.NewGuid().ToString("N");
            var user = await graphApi.GetUserFromToken();

            await repo.CreateUpdate(new UserActivity
            {
                Action = input.Action,
                Date = input.Date,
                EntityName = input.EntityName,
                Id = id,
                IdEntity = input.IdEntity,
                User = user
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
