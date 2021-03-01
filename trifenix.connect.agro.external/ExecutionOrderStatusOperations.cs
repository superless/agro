using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.agro.external.operations.entities.orders
{
    /// <summary>
    /// Operaciones de los estados de las ordenes ejecutadas,
    /// se encarga de almacenar los datos ingresados
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecutionOrderStatusOperations<T> : MainOperation<ExecutionOrderStatus, ExecutionOrderStatusInput,T>, IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> {
        public ExecutionOrderStatusOperations(IMainGenericDb<ExecutionOrderStatus> repo, IAgroSearch<T> search, IValidatorAttributes<ExecutionOrderStatusInput> validator) : base(repo, search, validator) { }

  

        

        public override async Task<ExtPostContainer<string>> SaveInput(ExecutionOrderStatusInput input) {
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
            await SaveDb(executionStatus);
            return await SaveSearch(executionStatus);
        }

       
    }

}