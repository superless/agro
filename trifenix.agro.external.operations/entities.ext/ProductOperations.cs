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
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.util.exceptions;

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

                return await OperationHelper.EditElement<Product>(id,
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
                 $"No existe producto con id : {id}"
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


        private async Task<List<T>> SelectElement<T>(IEnumerable<string> list, Func<string, Task<T>> getElement, string message) {
            if (list == null) throw new GenericException(message);
            if (list.Any(s => s == null)) throw new GenericException(message);
            var listLocal = new List<T>();
            foreach (var item in list)
            {
                var element = await getElement(item);
                if (element == null) throw new GenericException(message);
                listLocal.Add(element);
            }
            return listLocal;
        }
        private async Task<List<Doses>> GetDoses(DosesInput[] input, IEnumerable<string> varietyIds, IEnumerable<string> targetsId, IEnumerable<string> speciesIds, IEnumerable<string> certifiedEntitiesIds) {

            

            var varieties = await SelectElement(varietyIds, varietyRepository.GetVariety, "Una o más variedades no fueron encontradas");
            var targets = await SelectElement(targetsId, targetRepository.GetTarget, "Uno o mas objetivos de aplicación no fueron encontrados"); 
            var species = await SelectElement(speciesIds, specieRepository.GetSpecie, "Uno o mas especies no fueron encontrados");
            var certifiedEntities = await SelectElement(certifiedEntitiesIds, certifiedEntityRepository.GetCertifiedEntity,"uno o más de las entidades certificadoras no fueron encontradas");

            return input.Select(i => new Doses {
                ApplicationDaysInterval = i.ApplicationDaysInterval,
                DaysToReEntryToBarrack = i.DaysToReEntryToBarrack,
                NumberOfSecuencialApplication = i.NumberOfSecuencialAppication,
                Targets = i.idsApplicationTarget.Select(s=>targets.First(a=>a.Id.Equals(s))).ToList(),
                Varieties = i.IdVarieties.Select(s => varieties.First(a => a.Id.Equals(s))).ToList(),
                Species = i.IdSpecies.Select(s => species.First(a => a.Id.Equals(s))).ToList(),
                WettingRecommendedByHectares = i.WettingRecommendedByHectares,
                IdSeason = idSeason,
                DosesApplicatedTo = i.DosesApplicatedTo,
                DosesQuantityMin = i.DosesQuantityMin,
                DosesQuantityMax = i.DosesQuantityMax,
                WaitingDaysLabel = i.WaitingDaysLabel,
                WaitingToHarvest = i.WaitingHarvest.Select(w=>new WaitingHarvest {
                    CertifiedEntity = certifiedEntities.First(c=>c.Id.Equals(w.IdCertifiedEntity)),
                    WaitingDays = w.WaitingDays
                }).ToList()

            }).ToList();
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
