using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.ext
{
    public class CertifiedEntityOperations : MainOperation<CertifiedEntity, CertifiedEntityInput>, IGenericOperation<CertifiedEntity, CertifiedEntityInput> {
        public CertifiedEntityOperations(IMainGenericDb<CertifiedEntity> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonDbOperations<CertifiedEntity> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(CertifiedEntity certifiedEntity) {
            await repo.CreateUpdate(certifiedEntity);
            search.AddDocument(certifiedEntity);
            return new ExtPostContainer<string> {
                IdRelated = certifiedEntity.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(CertifiedEntityInput input, bool isBatch) {
            await Validate(input);
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