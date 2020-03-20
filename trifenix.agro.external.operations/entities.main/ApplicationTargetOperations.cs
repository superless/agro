using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{


    public class ApplicationTargetOperations : MainReadOperationName<ApplicationTarget, TargetInput>, IGenericOperation<ApplicationTarget, TargetInput>
    {
        public ApplicationTargetOperations(IMainGenericDb<ApplicationTarget> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<ApplicationTarget> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(TargetInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var target = new ApplicationTarget
            {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, target.CosmosEntityName));
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<ApplicationTarget>("Abbreviation", input.Abbreviation);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, target.CosmosEntityName));

            }
            else
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<ApplicationTarget>("Abbreviation", input.Abbreviation, input.Id);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, target.CosmosEntityName));
            }


            await repo.CreateUpdate(target);


            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.TARGET,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property{ 
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = input.Abbreviation
                        }
                    }
                }
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
