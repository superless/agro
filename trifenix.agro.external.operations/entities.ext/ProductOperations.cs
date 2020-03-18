using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext
{
    public class ProductOperations : MainOperation<Product, ProductInput>, IGenericOperation<Product, ProductInput> {
        private readonly IGenericOperation<Dose, DosesInput> dosesOperation;

        public ProductOperations(IMainGenericDb<Product> repo, IExistElement existElement, IAgroSearch search, IGenericOperation<Dose, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb) : base(repo, existElement, search, commonDb) {
            this.dosesOperation = dosesOperation;
        }
        
        private Property[] GetElementRelated(ProductInput input) {
            var properties = new Property[] {
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = input.Name },
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = input.Brand }
            };
            return properties;

        }

        public async Task<string> Validate(ProductInput input) {
            string errors = string.Empty;
            if (!string.IsNullOrWhiteSpace(input.Id)) {  //PUT
                var existsId = await existElement.ExistsById<Product>(input.Id, false);
                if (!existsId)
                    errors += $"No existe producto con id {input.Id}.";
            }
            if (input.Doses != null && input.Doses.Any()) {
                foreach(var dose in input.Doses) {
                    try {
                        await dosesOperation.Validate(dose,false);
                    } catch (Validation_Exception ex) {
                        errors += $"La dosis con id {dose.Id} tiene el(los) siguiente(s) problema(s): {string.Join(" ", ex.ErrorMessages)}";
                    }
                }
            }
            var existsName = await existElement.ExistsWithPropertyValue<Product>("Name", input.Name, input.Id, false);
            if (existsName)
                errors += $"Ya existe el nombre {input.Name}.";
            var existsIngredient = await existElement.ExistsById<Ingredient>(input.IdActiveIngredient, false);
            if (!existsIngredient)
                errors += $"No existe ingrediente con id {input.IdActiveIngredient}.";
            return errors.Replace(".",".\r\n");  
        }

        public async Task<ExtPostContainer<string>> Save(Product p) => null;

        public async Task<ExtPostContainer<string>> SaveInput(ProductInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var product = new Product {
                Id = id,
                Name = input.Name,
                IdActiveIngredient = input.IdActiveIngredient,
                Brand = input.Brand,
                MeasureType = input.MeasureType,
                Quantity = input.Quantity,
                KindOfBottle = input.KindOfBottle
            };
            //await repo.CreateUpdate(product, isBatch);
            if (input.Doses!= null && input.Doses.Any())
                foreach (var dose in input.Doses) {
                    dose.IdProduct = id;
                    await dosesOperation.SaveInput(dose, isBatch);
                }
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = id,
                    EntityIndex = (int)EntityRelated.PRODUCT,
                    Created = DateTime.Now,
                    RelatedProperties = GetElementRelated(input),
                    RelatedIds = new RelatedId[] { new RelatedId { EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = input.IdActiveIngredient } }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}