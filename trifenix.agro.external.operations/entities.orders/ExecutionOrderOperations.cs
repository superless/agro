using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations : MainReadOperation<ExecutionOrder>, IExecutionOrderOperations
    {
        private readonly ICommonQueries commonQueries;

        public ExecutionOrderOperations(IMainGenericDb<ExecutionOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
        }


        private async Task<string> ValidaExecutionOrder(ExecutionOrderInput input) { 
            
        
        }


        public async Task<ExtPostContainer<string>> Save(ExecutionOrderInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");


            

            var validaPreOrder = await ValidaOrder(input);

            if (!string.IsNullOrWhiteSpace(validaPreOrder)) throw new Exception(validaPreOrder);

            var order = new ApplicationOrder
            {
                Id = id,
                Barracks = input.Barracks,
                DosesOrder = input.DosesOrder,
                EndDate = input.EndDate,
                InitDate = input.InitDate,
                IdsPhenologicalPreOrder = input.IdsPhenologicalPreOrder,
                Name = input.Name,
                OrderType = input.OrderType,
                Wetting = input.Wetting
            };



            await repo.CreateUpdate(order);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(input.Barracks.First().IdBarrack);


            var entity = new EntitySearch
            {
                Id = id,
                EntityIndex = (int)EntityRelated.APPLICATION_ORDER,
                Created = DateTime.Now,
                RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = $"{input.InitDate : dd/MM/yyyy}"
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = $"{input.EndDate : dd/MM/yyyy}"
                        }
                    },
                RelatedEnumValues = new RelatedEnumValue[] {
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.ORDER_TYPE, Value = (int)input.OrderType }
                }
            };

            var entitiesForBarrack = input.Barracks.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.BARRACK, EntityId = s.IdBarrack }).ToList();

            var entitiesForEvents = input.Barracks.SelectMany(s => s.IdEvents).Distinct().Select(s => new RelatedId { EntityIndex = (int)EntityRelated.NOTIFICATION, EntityId = s });

            var entitiesForDoses = input.DosesOrder.Select(s => s.IdDoses).Distinct().Select(s => new RelatedId { EntityIndex = (int)EntityRelated.DOSES, EntityId = s });

            var entities = new List<RelatedId>();

            entities.AddRange(entitiesForBarrack);

            entities.AddRange(entitiesForEvents);

            entities.AddRange(entitiesForDoses);


            if (input.OrderType == OrderType.PHENOLOGICAL)
            {
                entities.AddRange(input.IdsPhenologicalPreOrder.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.PREORDER, EntityId = s }));
            }


            entity.RelatedIds = entities.ToArray();



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

        public Task<ExtPostContainer<ExecutionOrder>> SetStatus(string idExecutionOrder, string typeOfStatus, int newValueOfStatus, string commentary)
        {
            throw new NotImplementedException();
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