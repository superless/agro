using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations : MainReadOperation<ExecutionOrder>, IGenericOperation<ExecutionOrder, ExecutionOrderInput>
    {
        private readonly ICommonQueries commonQueries;

        public ExecutionOrderOperations(IMainGenericDb<ExecutionOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<ExecutionOrder> commonDb) : base(repo, existElement, search, commonDb)
        {
            this.commonQueries = commonQueries;
        }

        public async Task Remove(string id)
        {

        }
        private async Task<string> ValidaExecutionOrder(ExecutionOrderInput input) {

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var exists = await existElement.ExistsById<ExecutionOrder>(input.Id);
                if (!exists) return $"la ejecución con id {input.Id} no existe";
            }

            if (!input.DosesOrder.Any()) return "Debe existir al menos un producto";

            foreach (var doses in input.DosesOrder)
            {
                var exists = await existElement.ExistsById<Doses>(doses.IdDoses);
                if (!exists) return $"la dosis con id {doses.IdDoses} no existe";

            }


          

            if (input.InitDate > input.EndDate) return "La fecha inicial no puede ser mayor a la final";


            if (!string.IsNullOrWhiteSpace(input.IdUserApplicator))
            {
                var userExists = await existElement.ExistsById<User>(input.IdUserApplicator);
                if (!userExists) return $"usuario con id {input.IdUserApplicator} no existe en la base de datos"; 
            }

            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
            {
                var nebulizerExists = await existElement.ExistsById<Nebulizer>(input.IdNebulizer);
                if (!nebulizerExists) return $"nebulizador con id {input.IdNebulizer} no existe";

            }


            if (!string.IsNullOrWhiteSpace(input.IdTractor))
            {
                var tractorExists = await existElement.ExistsById<Tractor>(input.IdTractor);
                if (!tractorExists) return $"tractor con id {input.IdTractor} no existe";
            }


            var orderExists = await existElement.ExistsById<ApplicationOrder>(input.IdOrder);
            if (!orderExists) return $"orden con id {input.IdOrder} no existe en la base de datos";

            return string.Empty;

        }


        public async Task<ExtPostContainer<string>> Save(ExecutionOrderInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var validaPreOrder = await ValidaExecutionOrder(input);

            if (!string.IsNullOrWhiteSpace(validaPreOrder)) throw new Exception(validaPreOrder);

            var execution = new ExecutionOrder
            {
                Id= input.Id,
                IdUserApplicator = input.IdUserApplicator,
                IdNebulizer = input.IdNebulizer,
                IdOrder = input.IdOrder,
                IdTractor = input.IdTractor,
                InitDate = input.InitDate,
                EndDate = input.EndDate,
                DosesOrder = input.DosesOrder

            };

            
            await repo.CreateUpdate(execution);



            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromOrder(input.IdOrder);

            var properties = new List<Property>() {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        } 
            };

            if (input.InitDate.HasValue)
            {
                properties.Add(
                    new Property { 
                        PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                        Value = $"{input.InitDate.Value}:dd/MM/yyyy"
                    }
                );
            }

            if (input.EndDate.HasValue)
            {
                properties.Add(
                    new Property
                    {
                        PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                        Value = $"{input.EndDate.Value}:dd/MM/yyyy"
                    }
                );
            }


            var entity = new EntitySearch
            {
                Id = id,
                EntityIndex = (int)EntityRelated.EXECUTION_ORDER,
                Created = DateTime.Now,               
                RelatedProperties = properties.ToArray()

            };

            var entitiesIds = new List<RelatedId> {
                    new RelatedId{
                        EntityIndex = (int)EntityRelated.ORDER, EntityId = input.IdOrder
                    }
            };
            if (!string.IsNullOrWhiteSpace(input.IdUserApplicator))
            {
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.USER, EntityId = input.IdUserApplicator });
            }

            if (!string.IsNullOrWhiteSpace(input.IdNebulizer))
            {
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.NEBULIZER, EntityId = input.IdNebulizer });
            }




            //TODO : Eliminar antes de agregar
            foreach (var doses in input.DosesOrder)
            {
                var idGuid = Guid.NewGuid().ToString("N");
                var inputSearch = new EntitySearch
                {
                    Id = idGuid,
                    Created = DateTime.Now,
                    EntityIndex = (int)EntityRelated.DOSES_ORDER,
                    RelatedProperties = new Property[] {
                        new Property{ PropertyIndex = (int)PropertyRelated.GENERIC_QUANTITY_HECTARE,  Value = $"{doses.QuantityByHectare}" }
                     },
                    RelatedIds = new RelatedId[] {
                        new RelatedId{ EntityIndex=(int)EntityRelated.DOSES, EntityId = doses.IdDoses },
                        new RelatedId { EntityIndex = (int)EntityRelated.EXECUTION_ORDER, EntityId = id }
                     }

                };
                search.AddElements(new List<EntitySearch> {
                    inputSearch
                });
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.DOSES_ORDER, EntityId = idGuid });
            }

            if (!string.IsNullOrWhiteSpace(input.IdTractor))
            {
                entitiesIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.TRACTOR, EntityId = input.IdTractor });
            }


            entity.RelatedIds = entitiesIds.ToArray();

            search.AddElements(new List<EntitySearch> {
                entity
            });

            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

        
    }
}

/*Ejecucion
* Anadir campos (producto,cantidad) relacionados a la orden.
* Es necesario crear una ruta para obtener lista de usuarios aplicadores
* Es necesario crear una ruta para obtener ejecuciones en proceso (Transversal a las ordenes)
* Es necesario crear una ruta para obtener ordenes que contengan ejecuciones en proceso
* Cuando la ejecucion cambie su executionStatus a 1:InProcess, se copiaran la fecha inicial y final de la orden a si misma.
* Cada vez que se setee el executionStatus (Inicialmente y sus sucesivos cambios), se debe almacenar la fecha (ExecutionStatusDate)
* El nuevo executionStatus debe ser siempre igual o superior al anterior (Como maximo en una unidad, ya que este estado es serial)
* EL finishStatus solo se puede setear cuando el executionStatus tiene el valor 2:EndProcess
* El closedStatus solo se puede setear cuando el executionStatus tiene el valor 3:Closed
* Al crear una ejecucion (En planificacion) es obligatoria la orden relacionada.
* Al iniciar la ejecucion (En proceso) el usuario aplicador asignado es obligatorio.
* El closedStatus solo puede ser seteado si el usuario posee el rol de "Administrador".
* Comentarios para cada estado, independiente de los comentarios transversales.
* Si la ejecucion ya finalizo(finishStatus != 0) solo se pueden recibir comentarios y cierre de ejecucion(set closedStatus to != 0)
* Si la orden relacionada ya posee una ejecucion exitosa no se puede crear una nueva ejecucion.*/