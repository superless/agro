using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations : IExecutionOrderOperations
    {
        private readonly IExecutionOrderRepository _executionRepository;


        public ExecutionOrderOperations(
            IExecutionOrderRepository executionRepository)
        {
            _executionRepository = executionRepository;
        }
        public async Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrder(string id)
        {
            var executionOrder = await _executionRepository.GetExecutionOrder(id);
            return OperationHelper.GetElement(executionOrder);
        }

        public async Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor, string commentary)
        {
            if (string.IsNullOrWhiteSpace(idOrder)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'idOrder' para crear una ejecucion."));

            
            List<ProductToApply> productApplies;
            try
            {
                productApplies = GetProductsExecution(idsProduct, quantitiesByHectare);
            }
            catch (Exception e)
            {
                return OperationHelper.GetPostException<string>(e);
            }

            var execution = new ExecutionOrder
            {
                Id = Guid.NewGuid().ToString("N"),
                ClosedStatus = ClosedStatus.NotClosed,
                ExecutionStatus = ExecutionStatus.Planification,
                IdNebulizer = idNebulizer,
                IdOrder = idOrder,
                IdTractor = idTractor,
                ProductToApply = productApplies,
                StatusInfo = new string[4],
                FinishStatus = FinishStatus.NotFinish,
                IdUserApplicator = idUserApplicator
            };

            //execution
            var executionId =  await _executionRepository.CreateUpdateExecutionOrder(execution);



            return new ExtPostContainer<string>
            {
                IdRelated = executionId,
                Result = executionId,
                MessageResult = ExtMessageResult.Ok
            };
        }
        private List<ProductToApply> GetProductsExecution(string[] idsProduct, double[] quantities)
        {
            if (idsProduct.Count() != quantities.Count())
                throw new Exception("Los productos no cumplen el criterio.");


            var max = idsProduct.Count();
            var list = new List<ProductToApply>();
            for (int i = 0; i < max; i++)
            {

                var localQuantity = quantities[i];
                list.Add(new ProductToApply
                {
                    IdProduct = idsProduct[i],
                    QuantityByHectare = localQuantity
                });
            }
            return list;
        }

        public Task<ExtPostContainer<string>> SaveEditExecutionOrder(string idExecutionOrder, string idOrder, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor)
        {
            throw new NotImplementedException();
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