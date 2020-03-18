using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext {
    public class CertifiedEntityOperations : MainOperation<CertifiedEntity, CertifiedEntityInput>, IGenericOperation<CertifiedEntity, CertifiedEntityInput> {
        public CertifiedEntityOperations(IMainGenericDb<CertifiedEntity> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<CertifiedEntity> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(CertifiedEntity certifiedEntity) {
            await repo.CreateUpdate(certifiedEntity); 
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = certifiedEntity.Id,
                    EntityIndex = (int)EntityRelated.CERTIFIED_ENTITY,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = certifiedEntity.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = certifiedEntity.Abbreviation
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = certifiedEntity.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(CertifiedEntityInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var certifiedEntity = new CertifiedEntity {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(certifiedEntity);
            await repo.CreateEntityContainer(certifiedEntity);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}