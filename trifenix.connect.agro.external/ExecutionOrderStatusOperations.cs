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

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderStatusOperations<T> : MainOperation<ExecutionOrderStatus, ExecutionOrderStatusInput,T>, IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> {
        public ExecutionOrderStatusOperations(IMainGenericDb<ExecutionOrderStatus> repo, IAgroSearch<T> search, ICommonDbOperations<ExecutionOrderStatus> commonDb, IValidatorAttributes<ExecutionOrderStatusInput, ExecutionOrderStatus> validator) : base(repo, search, commonDb, validator) { }

  

        public async Task<ExtPostContainer<string>> Save(ExecutionOrderStatus executionOrderStatus) {
            await repo.CreateUpdate(executionOrderStatus);
            search.AddDocument(executionOrderStatus);
            return new ExtPostContainer<string> {
                IdRelated = executionOrderStatus.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ExecutionOrderStatusInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var executionStatus = new ExecutionOrderStatus {
                Id = id,
                ClosedStatus = input.ClosedStatus,
                ExecutionStatus = input.ExecutionStatus,
                Comment = input.Comment,
                Created = DateTime.Now,
                FinishStatus = input.FinishStatus,
                IdExecutionOrder = input.IdExecutionOrder
            };
            if (!isBatch)
                return await Save(executionStatus);
            await repo.CreateEntityContainer(executionStatus);
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