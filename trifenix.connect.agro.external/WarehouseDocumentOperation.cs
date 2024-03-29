﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.exception;

namespace trifenix.agro.external
{

    /// <summary>
    /// Operaciones para el ingreso correcto de un documento de bodega
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WarehouseDocumentOperations<T> : MainOperation<WarehouseDocument, WarehouseDocumentInput, T>, IGenericOperation<WarehouseDocument, WarehouseDocumentInput>
    {
        private readonly ICommonAgroQueries Queries;

        public WarehouseDocumentOperations(IDbExistsElements existsElement, IMainGenericDb<WarehouseDocument> repo, IAgroSearch<T> search, ICommonAgroQueries queries, IValidatorAttributes<WarehouseDocumentInput> validator, ILogger log) : base(repo, search, validator, log)
        {
            Queries = queries;
        }

        public async override Task Validate(WarehouseDocumentInput input)
        {
            if (!input.ProductDocuments.Any())
            {
                throw new CustomException("Tiene que haber al menos un producto");
            }
            else
            {
               var isUnique = input.ProductDocuments.GroupBy(o => o.IdProduct).Max(g => g.Count()) == 1;
               if (!isUnique)
               {
                    throw new CustomException("No se pueden ingresar productos repetidos");
               }
            }

            await base.Validate(input);

            if (!Enum.IsDefined(typeof(DocumentType), input.DocumentType))
                throw new ArgumentOutOfRangeException("input.DocumentType", "Enum fuera de rango");

            if (!Enum.IsDefined(typeof(PaymentType), input.PaymentType))
                throw new ArgumentOutOfRangeException("input.PaymentType", "Enum fuera de rango");

            if (!Enum.IsDefined(typeof(DocumentState), input.DocumentState))
                throw new ArgumentOutOfRangeException("input.DocumentState", "Enum fuera de rango");

            if (!input.Output)
            {
                // Documento de entrada, por lo que se debe ingresar un origen desde donde vienen los productos, ese origen debe
                // ser un proveedor, o sea, un business name que no posea centro de costos. El valor de destino es null, ya que 
                // el id de bodega previamente ingresado tomará ese lugar.  
                if (string.IsNullOrWhiteSpace(input.Source))
                {
                    throw new CustomException("Debe ingresar un origen");
                }
                input.Destiny = input.IdWarehouse;
                var provider = await Queries.GetCostCenterFromBusinessName(input.Source);
                if (provider.Any())
                {
                    throw new CustomException("El business name posee centro de costos, por lo que no puede ser un proveedor");
                }
            }
            if (input.Output)
            {
                // Documento de salida, por lo que se debe ingresar un destino haia donde irán los productos, ese destino debe ser 
                // un business name que si posea centro de costos, para poder realizar la transacción. En este caso, el valor de origen
                // es null, ya que el id de bodega previamente ingresado tomará ese lugar.
                if (string.IsNullOrWhiteSpace(input.Destiny))
                {
                    throw new CustomException("Debe ingresar un destino");
                }
                input.Source = input.IdWarehouse;
                var noprovider = await Queries.GetCostCenterFromBusinessName(input.Destiny);
                if (!noprovider.Any())
                {
                    throw new CustomException("El business name no posee centro de costos, por lo que es un proveedor");
                }
        }
    }

        public override async Task<ExtPostContainer<string>> SaveInput(WarehouseDocumentInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            await Validate(input);

            var warehouseDocument = new WarehouseDocument
            {
                Id = id,
                IdWarehouse = input.IdWarehouse,
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
                Destiny = input.Destiny,
                Source = input.Source
            };
            await SaveDb(warehouseDocument);
            return await SaveSearch(warehouseDocument);
        }
    }
}