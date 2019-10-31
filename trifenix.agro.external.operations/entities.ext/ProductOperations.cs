using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.ext
{
    public class ProductOperations : IProductOperations
    {
        private readonly IIngredientRepository ingredientRepository;

        public ProductOperations(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }
        public async Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, string[] idDoses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {
            if (string.IsNullOrWhiteSpace(commercialName)) return OperationHelper.GetPostException<string>(new Exception("nombre comercial obligatorio"));
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return OperationHelper.GetPostException<string>(new Exception("Ingrediente activo es obligatorio"));
            if (string.IsNullOrWhiteSpace(brand)) return OperationHelper.GetPostException<string>(new Exception("Marca del producto es obligatoria"));


            throw new NotImplementedException();
        }
    }
}
