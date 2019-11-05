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

        public ProductOperations(IIngredientRepository ingredientRepository, IProductRepository productRepository, IApplicationTargetRepository targetRepository, ICertifiedEntityRepository certifiedEntityRepository, IVarietyRepository varietyRepository, ISpecieRepository specieRepository, ICommonDbOperations<Product> commonDb)
        {
            this.ingredientRepository = ingredientRepository;
            this.productRepository = productRepository;
            this.targetRepository = targetRepository;
            this.certifiedEntityRepository = certifiedEntityRepository;
            this.varietyRepository = varietyRepository;
            this.specieRepository = specieRepository;
            this.commonDb = commonDb;
        }
        public async Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {
            if (string.IsNullOrWhiteSpace(commercialName)) return OperationHelper.GetPostException<string>(new Exception("nombre comercial obligatorio"));
            if (string.IsNullOrWhiteSpace(idActiveIngredient)) return OperationHelper.GetPostException<string>(new Exception("Ingrediente activo es obligatorio"));
            if (string.IsNullOrWhiteSpace(brand)) return OperationHelper.GetPostException<string>(new Exception("Marca del producto es obligatoria"));

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
                speciesIds = doses.Select(s => s.IdSpecie).Distinct();
                certifiedEntitiesIds = doses.SelectMany(s => s.WaitingToHarvest.Select(a => a.IdCertifiedEntity)).Distinct();

                localDoses = await GetDoses(doses, varietyIds, sicknessIds, speciesIds, certifiedEntitiesIds);
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
                       IdVarieties = varietyIds?.ToList()

                       
                   }),
                   s => s.CommercialName.Equals(commercialName),
                   $"ya existe producto con nombre : {commercialName}"
               );
        }
        private async Task<List<Doses>> GetDoses(DosesInput[] input, IEnumerable<string> varietyIds, IEnumerable<string> targetsId, IEnumerable<string> speciesIds, IEnumerable<string> certifiedEntitiesIds) {

            

            var varietyTasks = varietyIds.Select(varietyRepository.GetVariety);
            var targetsTasks = targetsId.Select(targetRepository.GetTarget);
            var speciesTasks = speciesIds.Select(specieRepository.GetSpecie);
            var certifiedTasks = certifiedEntitiesIds.Select(certifiedEntityRepository.GetCertifiedEntity);

            var varieties = await Task.WhenAll(varietyTasks);
            var targets = await Task.WhenAll(targetsTasks);
            var species = await Task.WhenAll(speciesTasks);
            var certifiedEntities = await Task.WhenAll(certifiedTasks);

            return input.Select(i => new Doses {
                ApplicationDaysInterval = i.ApplicationDaysInterval,
                DaysToReEntryToBarrack = i.DaysToReEntryToBarrack,
                NumberOfSecuencialAppication = i.NumberOfSecuencialAppication,
                Targets = i.idsApplicationTarget.Select(s=>targets.First(a=>a.Id.Equals(s))).ToList(),
                Varieties = i.IdVarieties.Select(s => varieties.First(a => a.Id.Equals(s))).ToList(),
                Specie = species.First(s=>s.Id.Equals(i.IdSpecie)),
                WettingRecommendedByHectares = i.WettingRecommended,
                WaitingToHarvest = i.WaitingToHarvest.Select(w=>new WaitingHarvest {
                    CertifiedEntity = certifiedEntities.First(c=>c.Id.Equals(w.IdCertifiedEntity)),
                    IsLabel = w.IsLabel,
                    WaitingDays = w.WaitingDays
                }).ToList()

            }).ToList();
        }
    }
}
