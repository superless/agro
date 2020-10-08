using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class CertifiedEntityOperations<T> : MainOperation<CertifiedEntity, CertifiedEntityInput,T>, IGenericOperation<CertifiedEntity, CertifiedEntityInput> {
        public CertifiedEntityOperations(IMainGenericDb<CertifiedEntity> repo,  IAgroSearch<T> search, ICommonDbOperations<CertifiedEntity> commonDb, IValidatorAttributes<CertifiedEntityInput, CertifiedEntity> validator) : base(repo, search, commonDb, validator) { }

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