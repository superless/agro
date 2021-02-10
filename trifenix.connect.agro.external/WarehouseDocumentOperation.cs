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

            if (string.IsNullOrWhiteSpace(input.IdWarehouse))
            {
                throw new Exception("Se ha ingresado un documento de bodega sin bodega asociada");
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
                IdWarehouse = warehouseDocumentInput.IdWarehouse,
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
                IdBusinessName = warehouseDocumentInput.IdBusinessName,
            };
            await SaveDb(warehouseDocument);
            return await SaveSearch(warehouseDocument);
        }
    }
}