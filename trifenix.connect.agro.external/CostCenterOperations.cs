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
    public class CostCenterOperations<T> : MainOperation<CostCenter, CostCenterInput,T>, IGenericOperation<CostCenter, CostCenterInput> {
        public CostCenterOperations(IMainGenericDb<CostCenter> repo, IAgroSearch<T> search, ICommonDbOperations<CostCenter> commonDb, IValidatorAttributes<CostCenterInput, CostCenter> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(CostCenter costCenter) {
            await repo.CreateUpdate(costCenter);
            search.AddDocument(costCenter);
            return new ExtPostContainer<string> {
                IdRelated = costCenter.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(CostCenterInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var costCenter = new CostCenter {
                Id = id,
                Name = input.Name,
                IdBusinessName = input.IdBusinessName
            };
            if (!isBatch)
                return await Save(costCenter);
            await repo.CreateEntityContainer(costCenter);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}