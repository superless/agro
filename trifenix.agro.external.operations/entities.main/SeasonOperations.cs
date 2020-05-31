using System;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.external.interfaces;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.search.interfaces;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.validator.interfaces;
using trifenix.agro.enums.input;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : MainOperation<Season, SeasonInput>, IGenericOperation<Season, SeasonInput> {
        public SeasonOperations(IMainGenericDb<Season> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Season> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public override async Task Validate(SeasonInput executionOrderStatusInput) {
            await base.Validate(executionOrderStatusInput);
        }

        public async Task<ExtPostContainer<string>> Save(Season season) {
            await repo.CreateUpdate(season);
            
            search.AddDocument(season);
            return new ExtPostContainer<string> {
                IdRelated = season.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SeasonInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var current = !input.Current.HasValue || input.Current.Value;
            var season = new Season {
                Id = id,
                Current = current,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                IdCostCenter = input.IdCostCenter
            };
            if (!isBatch)
                return await Save(season);
            await repo.CreateEntityContainer(season);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

    }

}