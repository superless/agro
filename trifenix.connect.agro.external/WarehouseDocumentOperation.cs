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
        private readonly ICommonAgroQueries Queries;

        public WarehouseDocumentOperations(IDbExistsElements existsElement, IMainGenericDb<WarehouseDocument> repo, IAgroSearch<T> search, ICommonDbOperations<WarehouseDocument> commonDb, ICommonAgroQueries queries, IValidatorAttributes<WarehouseDocumentInput> validator) : base(repo, search, commonDb, validator)
        {
            Queries = queries;
        }

        public async override Task Validate(WarehouseDocumentInput input)
        {

            if (!Enum.IsDefined(typeof(DocumentType), input.DocumentType))
                throw new ArgumentOutOfRangeException("input.DocumentType", "Enum fuera de rango");

            if (!Enum.IsDefined(typeof(PaymentType), input.PaymentType))
                throw new ArgumentOutOfRangeException("input.PaymentType", "Enum fuera de rango");

            if (!Enum.IsDefined(typeof(DocumentState), input.DocumentState))
                throw new ArgumentOutOfRangeException("input.DocumentState", "Enum fuera de rango");

            if (string.IsNullOrWhiteSpace(input.WHDestiny) || string.IsNullOrWhiteSpace(input.CCSource))
            {
                throw new ArgumentNullException("input.WHDestiny", "input.CCSource");
            }

            if (input.Output)
            {
                // Si output es true, significa que es un documento de entrada, por lo que se debe validar que el proveedor
                // no posea un cost center asociado.
                var providers = await Queries.GetBusinessNameIdFromCostCenter(input.CCSource);
                if (providers.Any())
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
                var providers = await Queries.GetBusinessNameIdFromCostCenter(input.CCSource);
                if (!providers.Any())
                {
                    throw new Exception("El business name posee centro de costos, por lo que no puede relizar la transaccion al ser un proveedor");
                }
            }
            else
            {
                await base.Validate(input);
            }
        }

        public override async Task<ExtPostContainer<string>> SaveInput(WarehouseDocumentInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            await Validate(input);

            var warehouseDocument = new WarehouseDocument
            {
                Id = id,
                WHDestiny = input.WHDestiny,
                DocumentType = input.DocumentType,
                EmissionDate = input.EmissionDate,
                PaymentType = input.PaymentType,
                DocumentState = input.DocumentState,
                Output = input.Output,
                ProductDocuments = input.ProductDocuments == null || !input.ProductDocuments.Any() ? new List<ProductDocument>() : input.ProductDocuments.Select(PD_Input => new ProductDocument
                {
                    IdProduct = PD_Input.IdProduct,
                    Quantity = PD_Input.Quantity,
                    Price = PD_Input.Price
                }).ToList(),
                CCSource = input.CCSource
            };
            await SaveDb(warehouseDocument);
            return await SaveSearch(warehouseDocument);
        }
    }
}