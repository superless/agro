using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class UserOperations : MainReadOperationName<UserApplicator, UserApplicatorInput>, IGenericOperation<UserApplicator, UserApplicatorInput>
    {
        private readonly IGraphApi graphApi;

        public UserOperations(IMainGenericDb<UserApplicator> repo, IExistElement existElement, IAgroSearch search, IGraphApi graphApi ) : base(repo, existElement, search)
        {
            this.graphApi = graphApi;
        }


        private IdsRelated[] GetIdsRelated(UserApplicator input) {

            var list = new List<IdsRelated>();
            if (!string.IsNullOrWhiteSpace(input.IdJob))
            {
                list.Add(new IdsRelated { 
                    EntityIndex = (int)EntityIdRelated.JOB,
                    EntityId = input.IdJob
                });
            }

            if (!string.IsNullOrWhiteSpace(input.IdTractor))
            {
                list.Add(new IdsRelated
                {
                    EntityIndex = (int)EntityIdRelated.TRACTOR,
                    EntityId = input.IdTractor
                });
            }

            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
            {
                list.Add(new IdsRelated
                {
                    EntityIndex = (int)EntityIdRelated.NEBULIZER,
                    EntityId = input.IdNebulizer
                });
            }
            if (input.IdsRoles != null && input.IdsRoles.Any())
            {
                foreach (var idRol in input.IdsRoles)
                {
                    list.Add(new IdsRelated
                    {
                        EntityIndex = (int)EntityIdRelated.ROLE,
                        EntityId = idRol
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(input.ObjectIdAAD))
            {
                list.Add(new IdsRelated
                {
                    EntityIndex = (int)EntityIdRelated.AAD,
                    EntityId = input.ObjectIdAAD
                });
            }

            return list.ToArray();
        }

        private ElementRelated[] GetElementRelated(UserApplicator input)
        {

            var list = new List<ElementRelated>();
            if (!string.IsNullOrWhiteSpace(input.Rut))
            {
                list.Add(new ElementRelated
                {
                    EntityIndex = (int)EntityRelated.USER_RUT,
                    Name = input.Rut
                });
            }

            if (!string.IsNullOrWhiteSpace(input.Email))
            {
                list.Add(new ElementRelated
                {
                    EntityIndex = (int)EntityRelated.USER_EMAIL,
                    Name = input.Email
                });
            }
            return list.ToArray();

        }



        public async Task<ExtPostContainer<string>> Save(UserApplicatorInput input)
        {

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, "Usuario"));
            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
            {
                var existsNebulizer = await existElement.ExistsElement<Nebulizer>(input.IdNebulizer);
                if (!existsNebulizer) throw new Exception(string.Format(ErrorMessages.NotValidId, "Nebulizador"));
            }
            if (!string.IsNullOrWhiteSpace(input.IdJob))
            {
                var existsJob = await existElement.ExistsElement<Job>(input.IdJob);
                if (!existsJob) throw new Exception(string.Format(ErrorMessages.NotValidId, "Cargo"));

            }

            if (!string.IsNullOrWhiteSpace(input.IdTractor))
            {
                var existsTractor = await existElement.ExistsElement<Tractor>(input.IdTractor);
                if (!existsTractor) throw new Exception(string.Format(ErrorMessages.NotValid, "Tractor"));
            }

            if (input.IdsRoles != null && input.IdsRoles.Any())
            {
                foreach (var idRol in input.IdsRoles)
                {
                    var exists = await existElement.ExistsElement<Role>(idRol);
                    
                    if (!exists) throw new Exception(string.Format(ErrorMessages.NotValid, "Rol"));
                }
            }


            UserApplicator userApp;
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var tmpUser = await Get(input.Id);
                userApp = tmpUser.Result;
                userApp.IdJob = input.IdJob;
                userApp.IdNebulizer = input.IdNebulizer;
                userApp.IdsRoles = input.IdsRoles;
                userApp.IdTractor = input.IdTractor;
                userApp.Name = input.Name;
                userApp.Rut = input.Rut;
            } else
            {
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

            search.AddEntities(new List<EntitySearch>
            {
                new EntitySearch{
                   EntityName = userApp.CosmosEntityName,
                   ElementsRelated = GetElementRelated(userApp),
                   Created = DateTime.Now,
                   Id = userApp.Id,
                   IdsRelated = GetIdsRelated(userApp),
                   Name = userApp.Name,
                }
            });


            return new ExtPostContainer<string>
            {
                IdRelated = userApp.Id,
                MessageResult = ExtMessageResult.Ok,
                Result = userApp.Id
            };
        }
    }
}