using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.orders
{
    /// <summary>
    /// Clase encargada de las operaciones de las ordenes ejecutadas,
    /// se encarga de la validacion y actualizacion de los parametros
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecutionOrderOperations<T> : MainOperation<ExecutionOrder, ExecutionOrderInput,T>, IGenericOperation<ExecutionOrder, ExecutionOrderInput> {
        private readonly ICommonAgroQueries commonQueries;

        public ExecutionOrderOperations(IMainGenericDb<ExecutionOrder> repo, IAgroSearch<T> search, ICommonAgroQueries commonQueries, ICommonDbOperations<ExecutionOrder> commonDb, IValidatorAttributes<ExecutionOrderInput> validator) : base(repo, search, commonDb, validator) {
            this.commonQueries = commonQueries;
        }

        public override async Task Validate(ExecutionOrderInput input) {
            await base.Validate(input);
            List<string> errors = new List<string>();

            if (!input.DosesOrder.Any())
                errors.Add("Debe existir al menos una dosis.");

            if (input.StartDate > input.EndDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
           
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        

        public override async Task<ExtPostContainer<string>> SaveInput(ExecutionOrderInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var execution = new ExecutionOrder {
                Id= id,
                IdUserApplicator = input.IdUserApplicator,
                IdNebulizer = input.IdNebulizer,
                IdOrder = input.IdOrder,
                IdTractor = input.IdTractor,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                DosesOrder = input.DosesOrder
            };
            await SaveDb(execution);
            return await SaveSearch(execution);

        }

    }

}