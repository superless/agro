using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.main {

    public class ApplicationTargetOperations : MainOperation<ApplicationTarget, ApplicationTargetInput>, IGenericOperation<ApplicationTarget, ApplicationTargetInput> {
        public ApplicationTargetOperations(IMainGenericDb<ApplicationTarget> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<ApplicationTarget> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

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