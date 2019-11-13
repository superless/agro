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

        private ExtPostErrorContainer<string> GetException(string message) => OperationHelper.GetPostException<string>(new Exception(message));
        public async Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {
            if (string.IsNullOrWhiteSpace(commercialName)) return GetException("nombre comercial obligatorio");
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return GetException("Ingrediente activo es obligatorio");
            if (string.IsNullOrWhiteSpace(brand)) return GetException("Marca del producto es obligatoria");

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
                    certifiedEntitiesIds = doses.SelectMany(s => s.WaitingToHarvest.Select(a => a.IdCertifiedEntity)).Distinct();

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

            

            var varieties = await SelectElement(varietyIds, varietyRepository.GetVariety, "");
            var targets = await SelectElement(targetsId, targetRepository.GetTarget, ""); 
            var species = await SelectElement(speciesIds, specieRepository.GetSpecie, "");
            var certifiedEntities = await SelectElement(certifiedEntitiesIds, certifiedEntityRepository.GetCertifiedEntity,"");

            return input.Select(i => new Doses {
                ApplicationDaysInterval = i.ApplicationDaysInterval,
                DaysToReEntryToBarrack = i.DaysToReEntryToBarrack,
                NumberOfSecuencialAppication = i.NumberOfSecuencialAppication,
                Targets = i.idsApplicationTarget.Select(s=>targets.First(a=>a.Id.Equals(s))).ToList(),
                Varieties = i.IdVarieties.Select(s => varieties.First(a => a.Id.Equals(s))).ToList(),
                Species = i.IdSpecies.Select(s => species.First(a => a.Id.Equals(s))).ToList(),
                WettingRecommendedByHectares = i.WettingRecommendedByHectares,
                IdSeason = idSeason,
                DosesApplicatedTo = i.DosesApplicatedTo,
                DosesQuantity = i.DosesQuantity,
                WaitingToHarvest = i.WaitingToHarvest.Select(w=>new WaitingHarvest {
                    CertifiedEntity = certifiedEntities.First(c=>c.Id.Equals(w.IdCertifiedEntity)),
                    IsLabel = w.IsLabel,
                    WaitingDays = w.WaitingDays
                }).ToList()

            }).ToList();
        }
    }

    
}
