using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations<T> : MainOperation<ExecutionOrder, ExecutionOrderInput,T>, IGenericOperation<ExecutionOrder, ExecutionOrderInput> {
        private readonly ICommonQueries commonQueries;

        public ExecutionOrderOperations(IMainGenericDb<ExecutionOrder> repo, IExistElement existElement, IAgroSearch<T> search, ICommonQueries commonQueries, ICommonDbOperations<ExecutionOrder> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.commonQueries = commonQueries;
        }

        public override async Task Validate(ExecutionOrderInput executionOrderInput) {
            await base.Validate(executionOrderInput);
            List<string> errors = new List<string>();
            if (!executionOrderInput.DosesOrder.Any())
                errors.Add("Debe existir al menos una dosis.");
            if (executionOrderInput.StartDate > executionOrderInput.EndDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        public async Task<ExtPostContainer<string>> Save(ExecutionOrder executionOrder) {
            await repo.CreateUpdate(executionOrder);
            search.AddDocument(executionOrder);
            return new ExtPostContainer<string> {
                IdRelated = executionOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ExecutionOrderInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var execution = new ExecutionOrder {
                Id= input.Id,
                IdUserApplicator = input.IdUserApplicator,
                IdNebulizer = input.IdNebulizer,
                IdOrder = input.IdOrder,
                IdTractor = input.IdTractor,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                DosesOrder = input.DosesOrder
            };
            if (!isBatch)
                return await Save(execution);
            await repo.CreateEntityContainer(execution);
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