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

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderStatusOperations : MainOperation<ExecutionOrderStatus, ExecutionOrderStatusInput>, IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> {
        public ExecutionOrderStatusOperations(IMainGenericDb<ExecutionOrderStatus> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonDbOperations<ExecutionOrderStatus> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        /*Ejecucion Status
        * El nuevo executionStatus debe ser siempre igual o superior al anterior (Como maximo en una unidad, ya que este estado es serial)
        * EL finishStatus solo se puede setear cuando el executionStatus tiene el valor 2:EndProcess
        * El closedStatus solo se puede setear cuando el executionStatus tiene el valor 3:Closed
        * Al crear una ejecucion (En planificacion) es obligatoria la orden relacionada.
        * Al iniciar la ejecucion (En proceso) el usuario aplicador asignado es obligatorio.
        * El closedStatus solo puede ser seteado si el usuario posee el rol de "Administrador".
        * Si la ejecucion ya finalizo(finishStatus != 0) solo se pueden recibir comentarios y cierre de ejecucion(set closedStatus to != 0)
        * Si la orden relacionada ya posee una ejecucion exitosa no se puede crear una nueva ejecucion.
        * Excelente Trabajo en documentar!!!!
         */        
        public override async Task Validate(ExecutionOrderStatusInput executionOrderStatusInput) {
            await base.Validate(executionOrderStatusInput);
        }

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