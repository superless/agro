using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.main {
    public class UserOperations : MainOperation<UserApplicator, UserApplicatorInput>, IGenericOperation<UserApplicator, UserApplicatorInput> {

        private readonly IGraphApi graphApi;

        public UserOperations(IMainGenericDb<UserApplicator> repo, IExistElement existElement, IAgroSearch search, IGraphApi graphApi, ICommonDbOperations<UserApplicator> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.graphApi = graphApi;
        }
        public async Task Remove(string id)
        {

        }
        private RelatedId[] GetIdsRelated(UserApplicator input) {
            var relatedIds = new List<RelatedId>();
            if (!string.IsNullOrWhiteSpace(input.IdJob))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.JOB, EntityId = input.IdJob });
            if (!string.IsNullOrWhiteSpace(input.IdTractor))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.TRACTOR, EntityId = input.IdTractor });
            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.NEBULIZER, EntityId = input.IdNebulizer });
            foreach (var idRole in input.IdsRoles)
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.ROLE, EntityId = idRole });
            return relatedIds.ToArray();
        }

        private Property[] GetPropertiesRelated(UserApplicator userApp) {
            var properties = new List<Property> {
                new Property { PropertyIndex = (int)PropertyRelated.OBJECT_ID_AAD, Value = userApp.ObjectIdAAD },
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = userApp.Name },
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_RUT, Value = userApp.Rut }
            };
            if (!string.IsNullOrWhiteSpace(userApp.Email))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_EMAIL, Value = userApp.Email });
            return properties.ToArray();
        }

        public async Task<ExtPostContainer<string>> Save(UserApplicator userApp) {
            await repo.CreateUpdate(userApp);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = userApp.Id,
                    EntityIndex = (int)EntityRelated.USER,
                    Created = DateTime.Now,
                    RelatedProperties = GetPropertiesRelated(userApp),
                    RelatedIds = GetIdsRelated(userApp)
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = userApp.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(UserApplicatorInput input, bool isBatch) {
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
            if (!isBatch)
                return await Save(user);
            await repo.CreateEntityContainer(user);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}