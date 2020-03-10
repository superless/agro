using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class UserOperations : MainReadOperationName<UserApplicator, UserApplicatorInput>, IGenericOperation<UserApplicator, UserApplicatorInput> {

        private readonly IGraphApi graphApi;

        public UserOperations(IMainGenericDb<UserApplicator> repo, IExistElement existElement, IAgroSearch search, IGraphApi graphApi, ICommonDbOperations<UserApplicator> commonDb) : base(repo, existElement, search, commonDb) {
            this.graphApi = graphApi;
        }

        private RelatedId[] GetIdsRelated(UserApplicator input) {
            var relatedIds = new List<RelatedId>();
            if (!string.IsNullOrWhiteSpace(input.IdJob))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.JOB, EntityId = input.IdJob });
            if (!string.IsNullOrWhiteSpace(input.IdTractor))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.TRACTOR, EntityId = input.IdTractor });
            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.NEBULIZER, EntityId = input.IdNebulizer });
            if (input.IdsRoles != null && input.IdsRoles.Any())
                foreach (var idRole in input.IdsRoles)
                    relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.ROLE, EntityId = idRole });
            return relatedIds.ToArray();
        }

        private Property[] GetPropertiesRelated(UserApplicator input) {
            var properties = new List<Property>();
            if (!string.IsNullOrWhiteSpace(input.ObjectIdAAD))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.OBJECT_ID_AAD, Value = input.ObjectIdAAD });
            if (!string.IsNullOrWhiteSpace(input.Name))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = input.Name });
            if (!string.IsNullOrWhiteSpace(input.Rut))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_RUT, Value = input.Rut });
            if (!string.IsNullOrWhiteSpace(input.Email))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_EMAIL, Value = input.Email });
            return properties.ToArray();
        }

        public async Task<ExtPostContainer<string>> Save(UserApplicatorInput input) {
            var valida = await Validate(input);
            if (!valida)
                throw new Exception(string.Format(ErrorMessages.NotValid, "Usuario"));
            if (!string.IsNullOrWhiteSpace(input.IdNebulizer)) {
                var existsNebulizer = await existElement.ExistsById<Nebulizer>(input.IdNebulizer);
                if (!existsNebulizer)
                    throw new Exception(string.Format(ErrorMessages.NotValidId, "Nebulizador"));
            }
            if (!string.IsNullOrWhiteSpace(input.IdJob)) {
                var existsJob = await existElement.ExistsById<Job>(input.IdJob);
                if (!existsJob)
                    throw new Exception(string.Format(ErrorMessages.NotValidId, "Cargo"));
            }
            if (!string.IsNullOrWhiteSpace(input.IdTractor)) {
                var existsTractor = await existElement.ExistsById<Tractor>(input.IdTractor);
                if (!existsTractor)
                    throw new Exception(string.Format(ErrorMessages.NotValid, "Tractor"));
            }
            if (input.IdsRoles != null && input.IdsRoles.Any())
                foreach (var idRol in input.IdsRoles) {
                    var exists = await existElement.ExistsById<Role>(idRol);                    
                    if (!exists)
                        throw new Exception(string.Format(ErrorMessages.NotValid, "Rol"));
                }
            UserApplicator userApp;
            if (!string.IsNullOrWhiteSpace(input.Id)) {
                var tmpUser = await Get(input.Id);
                userApp = tmpUser.Result;
                userApp.IdJob = input.IdJob;
                userApp.IdNebulizer = input.IdNebulizer;
                userApp.IdsRoles = input.IdsRoles;
                userApp.IdTractor = input.IdTractor;
                userApp.Name = input.Name;
                userApp.Rut = input.Rut;
                userApp.Email = input.Email;
            } else {
                var objectId = await graphApi.CreateUserIntoActiveDirectory(input.Name, input.Email);
                userApp = new UserApplicator { 
                    Email = input.Email,
                    Id = Guid.NewGuid().ToString("N"),
                    IdJob = input.IdJob,
                    IdNebulizer = input.IdNebulizer,
                    IdsRoles = input.IdsRoles,
                    Name = input.Name,
                    IdTractor = input.IdTractor,
                    Rut = input.Rut,
                    ObjectIdAAD = objectId
                };
            }
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
                MessageResult = ExtMessageResult.Ok,
                Result = userApp.Id
            };
        }

    }

}