using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.operations.common;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.entities.ext
{
    public class ProductOperations : IProductOperations
    {
        private readonly IIngredientRepository ingredientRepository;
        private readonly IProductRepository productRepository;
        private readonly IApplicationTargetRepository targetRepository;
        private readonly ICertifiedEntityRepository certifiedEntityRepository;
        private readonly IVarietyRepository varietyRepository;
        private readonly ISpecieRepository specieRepository;
        private readonly ICommonDbOperations<Product> commonDb;
        private readonly string idSeason;

        public ProductOperations(IIngredientRepository ingredientRepository, IProductRepository productRepository, IApplicationTargetRepository targetRepository, ICertifiedEntityRepository certifiedEntityRepository, IVarietyRepository varietyRepository, ISpecieRepository specieRepository, ICommonDbOperations<Product> commonDb, string idSeason)
        {
            this.ingredientRepository = ingredientRepository;
            this.productRepository = productRepository;
            this.targetRepository = targetRepository;
            this.certifiedEntityRepository = certifiedEntityRepository;
            this.varietyRepository = varietyRepository;
            this.specieRepository = specieRepository;
            this.commonDb = commonDb;
            this.idSeason = idSeason;
        }

        private ExtPostErrorContainer<T> GetException<T>(string message) => OperationHelper.GetPostException<T>(new Exception(message));

        public async Task<ExtPostContainer<Product>> CreateEditProduct(string id, string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {
            if (string.IsNullOrWhiteSpace(commercialName)) return GetException<Product>("nombre comercial obligatorio");
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return GetException<Product>("Ingrediente activo es obligatorio");
            if (string.IsNullOrWhiteSpace(brand)) return GetException<Product>("Marca del producto es obligatoria");

            try
            {
                var product = await productRepository.GetProduct(id);
                if (product == null)
                {
                    return OperationHelper.PostNotFoundElementException<Product>($"no se encontró producto con id {id}");
                }
                var ingredient = await ingredientRepository.GetIngredient(idActiveIngredient);

                if (ingredient == null)
                {
                    return OperationHelper.PostNotFoundElementException<Product>("No existe ingrediente", idActiveIngredient);
                }
                List<Doses> localDoses = null;
                IEnumerable<string> varietyIds = null;
                IEnumerable<string> sicknessIds = null;
                IEnumerable<string> speciesIds = null;
                IEnumerable<string> certifiedEntitiesIds = null;

                if (doses != null && doses.Any())
                {
                    varietyIds = doses.SelectMany(s => s.IdVarieties).Distinct();
                    sicknessIds = doses.SelectMany(s => s.idsApplicationTarget).Distinct();
                    speciesIds = doses.SelectMany(s => s.IdSpecies).Distinct();
                    certifiedEntitiesIds = doses.SelectMany(s => s.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct();

                    try
                    {
                        localDoses = await GetDoses(doses, varietyIds, sicknessIds, speciesIds, certifiedEntitiesIds);
                    }
                    catch (Exception e)
                    {

                        return OperationHelper.GetPostException<Product>(e);
                    }
                }

                return await OperationHelper.EditElement<Product>(commonDb, productRepository.GetProducts(), id,
                product,
                s => {
                    s.ActiveIngredient = ingredient;
                    s.Brand = brand;
                    s.CommercialName = commercialName;
                    s.IdsCertifiedEntities = certifiedEntitiesIds?.ToList();
                    s.IdsSpecies = speciesIds?.ToList();
                    s.IdsTargets = sicknessIds?.ToList();
                    s.IdVarieties = varietyIds.ToList();
                    s.KindOfContainer = kindOfProduct;
                    s.MeasureType = measureType;
                    s.QuantityByContainer = quantity;
                    s.Doses = localDoses;
                    return s;
                },
                productRepository.CreateUpdateProduct,
                 $"No existe producto con id : {id}",
                s => s.CommercialName.Equals(commercialName) && commercialName!=product.CommercialName,
                $"Este nombre comercial ya existe"
            );


            }
            catch (Exception e)
            {

                return OperationHelper.GetPostException<Product>(e);
            }

        }

        public async Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {
            if (string.IsNullOrWhiteSpace(commercialName)) return GetException<string>("nombre comercial obligatorio");
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return GetException<string>("Ingrediente activo es obligatorio");
            if (string.IsNullOrWhiteSpace(brand)) return GetException<string>("Marca del producto es obligatoria");

            try
            {

                
                var ingredient = await ingredientRepository.GetIngredient(idActiveIngredient);

                if (ingredient == null)
                {
                    return OperationHelper.PostNotFoundElementException<string>("No existe ingrediente", idActiveIngredient);
                }
                List<Doses> localDoses = null;
                IEnumerable<string> varietyIds = null;
                IEnumerable<string> sicknessIds = null;
                IEnumerable<string> speciesIds = null;
                IEnumerable<string> certifiedEntitiesIds = null;

                if (doses != null && doses.Any())
                {
                    varietyIds = doses.SelectMany(s => s.IdVarieties).Distinct();
                    sicknessIds = doses.SelectMany(s => s.idsApplicationTarget).Distinct();
                    speciesIds = doses.SelectMany(s => s.IdSpecies).Distinct();
                    certifiedEntitiesIds = doses.SelectMany(s => s.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct();

                    try
                    {
                        localDoses = await GetDoses(doses, varietyIds, sicknessIds, speciesIds, certifiedEntitiesIds);
                    }
                    catch (Exception e)
                    {

                        return OperationHelper.GetPostException<string>(e);
                    }
                }



                return await OperationHelper.CreateElement(commonDb, productRepository.GetProducts(),
                       async s => await productRepository.CreateUpdateProduct(new Product
                       {
                           Id = s,
                           ActiveIngredient = ingredient,
                           Brand = brand,
                           CommercialName = commercialName,
                           Doses = localDoses,
                           IdsCertifiedEntities = certifiedEntitiesIds?.ToList(),
                           IdsTargets = sicknessIds?.ToList(),
                           IdsSpecies = speciesIds?.ToList(),
                           IdVarieties = varietyIds?.ToList(),
                           KindOfContainer = kindOfProduct,
                           QuantityByContainer = quantity,
                           MeasureType = measureType
                       }),
                       s => s.CommercialName.Equals(commercialName),
                       $"ya existe producto con nombre : {commercialName}"
                   );
            }
            catch (Exception e)
            {

                return OperationHelper.GetPostException<string>(e);
            }
        }


       
        private async Task<List<Doses>> GetDoses(DosesInput[] input, IEnumerable<string> varietyIds, IEnumerable<string> targetsId, IEnumerable<string> speciesIds, IEnumerable<string> certifiedEntitiesIds) {


            return await ModelCommonOperations.GetDoses(varietyRepository, targetRepository, 
                specieRepository, certifiedEntityRepository, input, varietyIds, targetsId, speciesIds, certifiedEntitiesIds, idSeason);
        }

        

        public async Task<ExtGetContainer<List<Product>>> GetProducts()
        {
            try
            {
                var productsQuery = productRepository.GetProducts();
                var products = await commonDb.TolistAsync(productsQuery);
                return OperationHelper.GetElements(products);
            }
            catch (Exception e)
            {
                return OperationHelper.GetException<List<Product>>(e, e.Message);
            }
        }

        public async Task<ExtGetContainer<Product>> GetProduct(string id)
        {
            try
            {
                var product = await productRepository.GetProduct(id);
                return OperationHelper.GetElement(product);
            }
            catch (Exception e)
            {
                return OperationHelper.GetException<Product>(e, e.Message);
            }
        }
    }

    
}
