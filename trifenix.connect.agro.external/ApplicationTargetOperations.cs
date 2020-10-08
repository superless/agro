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

    public class ApplicationTargetOperations<T> : MainOperation<ApplicationTarget, ApplicationTargetInput,T>, IGenericOperation<ApplicationTarget, ApplicationTargetInput> {
        public ApplicationTargetOperations(IMainGenericDb<ApplicationTarget> repo, IAgroSearch<T> search, ICommonDbOperations<ApplicationTarget> commonDb, IValidatorAttributes<ApplicationTargetInput, ApplicationTarget> validator) : base(repo, search, commonDb, validator) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(ApplicationTarget applicationTarget) {
            await repo.CreateUpdate(applicationTarget);
            search.AddDocument(applicationTarget);
            return new ExtPostContainer<string> {
                IdRelated = applicationTarget.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ApplicationTargetInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var target = new ApplicationTarget {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(target);
            await repo.CreateEntityContainer(target);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}