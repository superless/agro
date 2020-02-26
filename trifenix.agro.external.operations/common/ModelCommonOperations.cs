using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.model.external.Input;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.common
{
    public static class ModelCommonOperations
    {

       
        public static async Task<List<Doses>> GetDoses(IVarietyRepository varietyRepository, IApplicationTargetRepository targetRepository, ISpecieRepository specieRepository, ICertifiedEntityRepository certifiedRepository,  DosesInput[] input, IEnumerable<string> varietyIds, IEnumerable<string> targetsId, IEnumerable<string> speciesIds, IEnumerable<string> certifiedEntitiesIds, string idSeason)
        {
            var varieties = await varietyIds.SelectElement(varietyRepository.GetVariety, "Una o más variedades no fueron encontradas");
            var targets = await targetsId.SelectElement(targetRepository.GetTarget, "Uno o mas objetivos de aplicación no fueron encontrados");
            var species = await speciesIds.SelectElement(specieRepository.GetSpecie, "Uno o mas especies no fueron encontrados");
            var certifiedEntities = await certifiedEntitiesIds.SelectElement(certifiedRepository.GetCertifiedEntity, "uno o más de las entidades certificadoras no fueron encontradas");

            return input.Select(i => new Doses
            {
                ApplicationDaysInterval = i.ApplicationDaysInterval,
                DaysToReEntryToBarrack = i.DaysToReEntryToBarrack,
                NumberOfSequentialApplication = i.NumberOfSequentialApplication,
                Targets = i.idsApplicationTarget.Select(s => targets.First(a => a.Id.Equals(s))).ToList(),
                Varieties = i.IdVarieties.Select(s => varieties.First(a => a.Id.Equals(s))).ToList(),
                Species = i.IdSpecies.Select(s => species.First(a => a.Id.Equals(s))).ToList(),
                WettingRecommendedByHectares = i.WettingRecommendedByHectares,
                
                DosesApplicatedTo = i.DosesApplicatedTo,
                DosesQuantityMin = i.DosesQuantityMin,
                DosesQuantityMax = i.DosesQuantityMax,
                WaitingDaysLabel = i.WaitingDaysLabel,
                WaitingToHarvest = i.WaitingHarvest.Select(w => new WaitingHarvest
                {
                    CertifiedEntity = certifiedEntities.First(c => c.Id.Equals(w.IdCertifiedEntity)),
                    WaitingDays = w.WaitingDays
                }).ToList()

            }).ToList();
        }

    }
}
