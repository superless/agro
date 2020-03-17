using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders {
    public class ExecutionOrderOperations : MainOperation<ExecutionOrder, ExecutionOrderInput>, IGenericOperation<ExecutionOrder, ExecutionOrderInput> {
        private readonly ICommonQueries commonQueries;

        public ExecutionOrderOperations(IMainGenericDb<ExecutionOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<ExecutionOrder> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
        }

        public override async Task Validate(ExecutionOrderInput executionOrderInput, bool isBatch) {
            await base.Validate(executionOrderInput, isBatch);
            List<string> errors = new List<string>();
            if (!executionOrderInput.DosesOrder.Any())
                errors.Add("Debe existir al menos una dosis.");
            if (executionOrderInput.InitDate > executionOrderInput.EndDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        public async Task<ExtPostContainer<string>> Save(ExecutionOrder executionOrder) {
            await repo.CreateUpdate(executionOrder, false);

            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromOrder(executionOrder.IdOrder);
            var properties = new List<Property>() { new Property { PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION, Value = specieAbbv } };
            if (executionOrder.InitDate.HasValue)
                properties.Add( new Property { PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE, Value = $"{executionOrder.InitDate.Value}:dd/MM/yyyy" });
            if (executionOrder.EndDate.HasValue)
                properties.Add( new Property { PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE, Value = $"{executionOrder.EndDate.Value}:dd/MM/yyyy" });

            var entitiesIds = new List<RelatedId> { new RelatedId { EntityIndex = (int)EntityRelated.ORDER, EntityId = executionOrder.IdOrder } };
            if (!string.IsNullOrWhiteSpace(executionOrder.IdUserApplicator))
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.USER, EntityId = executionOrder.IdUserApplicator });
            if (!string.IsNullOrWhiteSpace(executionOrder.IdNebulizer))
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.NEBULIZER, EntityId = executionOrder.IdNebulizer });
            if (!string.IsNullOrWhiteSpace(executionOrder.IdTractor))
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.TRACTOR, EntityId = executionOrder.IdTractor });

            //TODO : Eliminar antes de agregar
            string idGuid;
            foreach (var doses in executionOrder.DosesOrder) {
                idGuid = Guid.NewGuid().ToString("N");
                search.AddElements(new List<EntitySearch> {
                    new EntitySearch {
                        Id = idGuid,
                        Created = DateTime.Now,
                        EntityIndex = (int)EntityRelated.DOSES_ORDER,
                        RelatedProperties = new Property[] { new Property{ PropertyIndex = (int)PropertyRelated.GENERIC_QUANTITY_HECTARE,  Value = $"{doses.QuantityByHectare}" } },
                        RelatedIds = new RelatedId[] {
                            new RelatedId { EntityIndex=(int)EntityRelated.DOSES, EntityId = doses.IdDoses },
                            new RelatedId { EntityIndex = (int)EntityRelated.EXECUTION_ORDER, EntityId = executionOrder.Id }
                        }
                    }
                });
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.DOSES_ORDER, EntityId = idGuid });
            }
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = executionOrder.Id,
                    EntityIndex = (int)EntityRelated.EXECUTION_ORDER,
                    Created = DateTime.Now,
                    RelatedProperties = properties.ToArray(),
                    RelatedIds = entitiesIds.ToArray()
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = executionOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ExecutionOrderInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var execution = new ExecutionOrder {
                Id= input.Id,
                IdUserApplicator = input.IdUserApplicator,
                IdNebulizer = input.IdNebulizer,
                IdOrder = input.IdOrder,
                IdTractor = input.IdTractor,
                InitDate = input.InitDate,
                EndDate = input.EndDate,
                DosesOrder = input.DosesOrder
            };
            if (!isBatch)
                return await Save(execution);
            await repo.CreateUpdate(execution, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}

/*Ejecucion
* El nuevo executionStatus debe ser siempre igual o superior al anterior (Como maximo en una unidad, ya que este estado es serial)
* EL finishStatus solo se puede setear cuando el executionStatus tiene el valor 2:EndProcess
* El closedStatus solo se puede setear cuando el executionStatus tiene el valor 3:Closed
* Al crear una ejecucion (En planificacion) es obligatoria la orden relacionada.
* Al iniciar la ejecucion (En proceso) el usuario aplicador asignado es obligatorio.
* El closedStatus solo puede ser seteado si el usuario posee el rol de "Administrador".
* Si la ejecucion ya finalizo(finishStatus != 0) solo se pueden recibir comentarios y cierre de ejecucion(set closedStatus to != 0)
* Si la orden relacionada ya posee una ejecucion exitosa no se puede crear una nueva ejecucion.*/