using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext
{
    public class CertifiedEntityOperations : MainReadOperationName<CertifiedEntity, CertifiedEntityInput>, IGenericOperation<CertifiedEntity, CertifiedEntityInput>
    {
        public CertifiedEntityOperations(IMainGenericDb<CertifiedEntity> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }


        public async Task<ExtPostContainer<string>> Save(CertifiedEntityInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var certifiedEntity = new CertifiedEntity
            {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, certifiedEntity.CosmosEntityName));

            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var validaAbbv = await existElement.ExistsElement<CertifiedEntity>("Abbreviation", input.Abbreviation);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, certifiedEntity.CosmosEntityName));

            }
            else
            {
                var validaAbbv = await existElement.ExistsEditElement<CertifiedEntity>(input.Id, "Abbreviation", input.Abbreviation);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, certifiedEntity.CosmosEntityName));
            }

            await repo.CreateUpdate(certifiedEntity);

            search.AddSimpleEntities(new List<SimpleSearch>
            {
                new SimpleSearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Name,
                    Abbreviation = input.Abbreviation,
                    EntityName = certifiedEntity.CosmosEntityName
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
