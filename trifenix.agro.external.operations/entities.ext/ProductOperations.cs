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
using trifenix.agro.model.external.output;
using trifenix.agro.search;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext
{
    public class ProductOperations : IProductOperations {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IApplicationTargetRepository _targetRepository;
        private readonly ICertifiedEntityRepository _certifiedEntityRepository;
        private readonly IVarietyRepository _varietyRepository;
        private readonly ISpecieRepository _specieRepository;
        private readonly ICommonDbOperations<Product> _commonDb;
        private readonly string _idSeason;
        private readonly AgroSearch _searchServiceInstance;
        private readonly string entityName = "Product";

        public ProductOperations(IIngredientRepository ingredientRepository, IProductRepository productRepository, IApplicationTargetRepository targetRepository, ICertifiedEntityRepository certifiedEntityRepository, IVarietyRepository varietyRepository, ISpecieRepository specieRepository, ICommonDbOperations<Product> commonDb, string idSeason, AgroSearch searchServiceInstance) {
            _ingredientRepository = ingredientRepository;
            _productRepository = productRepository;
            _targetRepository = targetRepository;
            _certifiedEntityRepository = certifiedEntityRepository;
            _varietyRepository = varietyRepository;
            _specieRepository = specieRepository;
            _commonDb = commonDb;
            _idSeason = idSeason;
            _searchServiceInstance = searchServiceInstance;
        }

        private ExtPostErrorContainer<T> GetException<T>(string message) => OperationHelper.GetPostException<T>(new Exception(message));

        public async Task<ExtPostContainer<Product>> CreateEditProduct(string id, string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct) {
            if (string.IsNullOrWhiteSpace(commercialName)) return GetException<Product>("nombre comercial obligatorio");
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return GetException<Product>("Ingrediente activo es obligatorio");
            if (string.IsNullOrWhiteSpace(brand)) return GetException<Product>("Marca del producto es obligatoria");
            try {
                var product = await _productRepository.GetProduct(id);
                if (product == null)
                    return OperationHelper.PostNotFoundElementException<Product>($"no se encontró producto con id {id}");
                var ingredient = await _ingredientRepository.GetIngredient(idActiveIngredient);
                if (ingredient == null)
                    return OperationHelper.PostNotFoundElementException<Product>("No existe ingrediente", idActiveIngredient);
                List<Doses> localDoses = null;
                IEnumerable<string> varietyIds = null;
                IEnumerable<string> sicknessIds = null;
                IEnumerable<string> speciesIds = null;
                IEnumerable<string> certifiedEntitiesIds = null;
                if (doses != null && doses.Any()) {
                    varietyIds = doses.SelectMany(s => s.IdVarieties).Distinct();
                    sicknessIds = doses.SelectMany(s => s.idsApplicationTarget).Distinct();
                    speciesIds = doses.SelectMany(s => s.IdSpecies).Distinct();
                    certifiedEntitiesIds = doses.SelectMany(s => s.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct();
                    try {
                        localDoses = await GetDoses(doses, varietyIds, sicknessIds, speciesIds, certifiedEntitiesIds);
                    }
                    catch (Exception e) {
                        return OperationHelper.GetPostException<Product>(e);
                    }
                }
                return await OperationHelper.EditElement<Product>(_commonDb, _productRepository.GetProducts(), id,
                    product,
                    s => {
                        _searchServiceInstance.AddEntities(new List<EntitySearch> {
                            new EntitySearch{
                                Created = DateTime.Now,
                                EntityName = entityName,
                                Name = commercialName,
                                Id = s.Id
                            }
                        });
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
                    _productRepository.CreateUpdateProduct,
                     $"No existe producto con id : {id}",
                    s => s.CommercialName.Equals(commercialName) && commercialName!=product.CommercialName,
                    $"Este nombre comercial ya existe");
            }
            catch (Exception e) {
                return OperationHelper.GetPostException<Product>(e);
            }
        }

        public async Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct) {
            if (string.IsNullOrWhiteSpace(commercialName)) return GetException<string>("nombre comercial obligatorio");
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return GetException<string>("Ingrediente activo es obligatorio");
            if (string.IsNullOrWhiteSpace(brand)) return GetException<string>("Marca del producto es obligatoria");
            try {
                var ingredient = await _ingredientRepository.GetIngredient(idActiveIngredient);
                if (ingredient == null)
                    return OperationHelper.PostNotFoundElementException<string>("No existe ingrediente", idActiveIngredient);
                List<Doses> localDoses = null;
                IEnumerable<string> varietyIds = null;
                IEnumerable<string> sicknessIds = null;
                IEnumerable<string> speciesIds = null;
                IEnumerable<string> certifiedEntitiesIds = null;
                if (doses != null && doses.Any()) {
                    varietyIds = doses.SelectMany(s => s.IdVarieties).Distinct();
                    sicknessIds = doses.SelectMany(s => s.idsApplicationTarget).Distinct();
                    speciesIds = doses.SelectMany(s => s.IdSpecies).Distinct();
                    certifiedEntitiesIds = doses.SelectMany(s => s.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct();
                    try {
                        localDoses = await GetDoses(doses, varietyIds, sicknessIds, speciesIds, certifiedEntitiesIds);
                    }
                    catch (Exception e) {
                        return OperationHelper.GetPostException<string>(e);
                    }
                }
                var id = await OperationHelper.CreateElement(_commonDb, _productRepository.GetProducts(),
                       async s => await _productRepository.CreateUpdateProduct(new Product {
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
                       $"ya existe producto con nombre : {commercialName}");
                _searchServiceInstance.AddEntities(new List<EntitySearch> {
                    new EntitySearch{
                        Created = DateTime.Now,
                        EntityName = entityName,
                        Name = commercialName,
                        Id = id.IdRelated
                    }
                    });
                return id;
            }
            catch (Exception e) {
                return OperationHelper.GetPostException<string>(e);
            }
        }

        private async Task<List<Doses>> GetDoses(DosesInput[] input, IEnumerable<string> varietyIds, IEnumerable<string> targetsId, IEnumerable<string> speciesIds, IEnumerable<string> certifiedEntitiesIds) {
            return await ModelCommonOperations.GetDoses(_varietyRepository, _targetRepository, _specieRepository, _certifiedEntityRepository, input, varietyIds, targetsId, speciesIds, certifiedEntitiesIds, _idSeason);
        }
        
        public async Task<ExtGetContainer<List<Product>>> GetProducts() {
            try {
                var productsQuery = _productRepository.GetProducts();
                var products = await _commonDb.TolistAsync(productsQuery);
                return OperationHelper.GetElements(products);
            }
            catch (Exception e) {
                return OperationHelper.GetException<List<Product>>(e);
            }
        }

        public async Task<ExtGetContainer<Product>> GetProduct(string id) {
            try {
                var product = await _productRepository.GetProduct(id);
                return OperationHelper.GetElement(product);
            }
            catch (Exception e) {
                return OperationHelper.GetException<Product>(e);
            }
        }

        public async Task<ExtGetContainer<SearchResult<Product>>> GetProductsByPage(int page, int quantity, bool orderByDesc) {
            try {
                var productsQuery = _productRepository.GetProducts();
                var paginatedProducts = _commonDb.WithPagination(productsQuery, page, quantity);
                var products = orderByDesc ? await _commonDb.TolistAsync(paginatedProducts.OrderByDescending(s => s.CommercialName)) : await _commonDb.TolistAsync(paginatedProducts);
                var total = productsQuery.Count();
                return OperationHelper.GetElement(new SearchResult<Product> {
                    Total = total,
                    Elements = products.ToArray()
                });
            }
            catch (Exception e) {
                return OperationHelper.GetException<SearchResult<Product>>(e);
            }
        }

        public async Task<ExtGetContainer<SearchResult<Product>>> GetProductsByPage(string textToSearch, int page, int quantity, bool desc) {
            if (string.IsNullOrWhiteSpace(textToSearch))
                return await GetProductsByPage(page, quantity, desc);
            var filters = new Filters { EntityName = entityName };
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetSearchFilteredByEntityName(filters, textToSearch, page, quantity, desc);
            var resultDb = entitySearch.Entities.Select(async s => await GetProduct(s.Id));
            return OperationHelper.GetElement(new SearchResult<Product> {
                Total = entitySearch.Total,
                Elements = resultDb.Select(s => s.Result.Result).ToArray()
            });
        }

        public ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, int page, int quantity, bool desc) {
            var filters = new Filters { EntityName = entityName };
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetSearchFilteredByEntityName(filters, textToSearch, page, quantity, desc);
            return OperationHelper.GetElement(entitySearch);
        }
    }
    
}
