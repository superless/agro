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
    public class SeasonOperations<T> : MainOperation<Season, SeasonInput,T>, IGenericOperation<Season, SeasonInput> {
        public SeasonOperations(IMainGenericDb<Season> repo, IAgroSearch<T> search, ICommonDbOperations<Season> commonDb, IValidatorAttributes<SeasonInput, Season> validator) : base(repo, search, commonDb, validator) { }

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