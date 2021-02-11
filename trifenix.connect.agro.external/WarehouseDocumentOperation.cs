using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.model;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.agro.external
{

    /// <summary>
    /// Operaciones para el ingreso correcto de un documento de bodega
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WarehouseDocumentOperations<T> : MainOperation<WarehouseDocument, WarehouseDocumentInput, T>, IGenericOperation<WarehouseDocument, WarehouseDocumentInput>
    {
        private readonly IDbExistsElements existsElement;
        private readonly ICommonAgroQueries Queries;

        public WarehouseDocumentOperations(IDbExistsElements existsElement, IMainGenericDb<WarehouseDocument> repo, IAgroSearch<T> search, ICommonDbOperations<WarehouseDocument> commonDb, ICommonAgroQueries queries, IValidatorAttributes<WarehouseDocumentInput> validator) : base(repo, search, commonDb, validator)
        {
            this.existsElement = existsElement;
            Queries = queries;
        }

        public async override Task Validate(WarehouseDocumentInput input)
        {
            if (!Enum.IsDefined(typeof(DocumentType), input.DocumentType))
                throw new ArgumentOutOfRangeException();

            if (!Enum.IsDefined(typeof(PaymentType), input.PaymentType))
                throw new ArgumentOutOfRangeException();

            if (!Enum.IsDefined(typeof(DocumentState), input.DocumentState))
                throw new ArgumentOutOfRangeException();

            if (string.IsNullOrWhiteSpace(input.WHDestiny) || string.IsNullOrWhiteSpace(input.CCSource))
            {
                throw new Exception("Se ha ingresado un documento de bodega sin fuente o destino");
            }

            if (input.Output)
            {
                // Si output es true, significa que es un documento de entrada, por lo que se debe validar que el proveedor
                // no posea un cost center asociado.
                var proveedores = await Queries.GetBusinessNameIdFromCostCenter(input.CCSource);
                if (proveedores.Any())
                {
                    throw new Exception("El business name posee centro de costos, por lo que no puede ser un proveedor");
                }
            }
            if (!input.Output)
            {
                // Si output es false, significa que es un documento de salida, por lo que la bodega asociada pasa a ser el origen 
                // de la transacción, y el business name, el destino. Se debe validar que el business name que está como destino
                // posea un centro de costos asociado. 
                var temp = input.WHDestiny;
                input.WHDestiny = input.CCSource;
                input.CCSource = temp;
                var proveedoresO = await Queries.GetBusinessNameIdFromCostCenter(input.CCSource);
                if (!proveedoresO.Any())
                {
                    throw new Exception("El business name posee no posee centro de costos, por lo que no puede ser realizado el traspaso");
                }
            }
            else
            {
                await base.Validate(input);
            }
        }

        public override async Task<ExtPostContainer<string>> SaveInput(WarehouseDocumentInput warehouseDocumentInput)
        {
            var id = !string.IsNullOrWhiteSpace(warehouseDocumentInput.Id) ? warehouseDocumentInput.Id : Guid.NewGuid().ToString("N");

            await Validate(warehouseDocumentInput);

            var warehouseDocument = new WarehouseDocument
            {
                Id = id,
                WHDestiny = warehouseDocumentInput.WHDestiny,
                DocumentType = warehouseDocumentInput.DocumentType,
                EmissionDate = warehouseDocumentInput.EmissionDate,
                PaymentType = warehouseDocumentInput.PaymentType,
                DocumentState = warehouseDocumentInput.DocumentState,
                Output = warehouseDocumentInput.Output,
                ProductDocuments = warehouseDocumentInput.ProductDocuments == null || !warehouseDocumentInput.ProductDocuments.Any() ? new List<ProductDocument>() : warehouseDocumentInput.ProductDocuments.Select(PD_Input => new ProductDocument
                {
                    IdProduct = PD_Input.IdProduct,
                    Quantity = PD_Input.Quantity,
                    Price = PD_Input.Price
                }).ToList(),
                CCSource = warehouseDocumentInput.CCSource
            };
            await SaveDb(warehouseDocument);
            return await SaveSearch(warehouseDocument);
        }
    }
}