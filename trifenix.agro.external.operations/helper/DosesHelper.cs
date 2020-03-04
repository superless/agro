using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.operations.helper
{
    public static class DosesHelper
    {
        public static string ValidaDoses(IExistElement existsElement, DosesInput dose) {
            

            if (dose.IdSpecies.Any())
            {
                var existsSpeciesId = dose.IdSpecies.Select(async s => await existsElement.ExistsById<Specie>(s));

                if (existsSpeciesId.Any(s => !s.Result))
                {
                    return "Uno o más de los id de specie no existe";
                }
            }

            if (dose.IdVarieties.Any())
            {
                var existsVarietiesId = dose.IdVarieties.Distinct().Select(async s => await existsElement.ExistsById<Variety>(s));

                if (existsVarietiesId.Any(s => !s.Result))
                {
                    return "Uno o más de los id de variedades no existe";
                }

            }

            if (dose.idsApplicationTarget.Any())
            {
                var existsTargetsId = dose.idsApplicationTarget.Distinct().Select(async s => await existsElement.ExistsById<ApplicationTarget>(s));

                if (existsTargetsId.Any(s => !s.Result))
                {
                    return "Uno o más de los id de objetivo no existe";
                }

            }

            
            if (dose.WaitingToHarvest.Any())
            {
                var certifiedIds = dose.WaitingToHarvest.Select(s => s.IdCertifiedEntity);
                if (certifiedIds.Any())
                {
                    var existsCertifiedsId = certifiedIds.Distinct().Select(async s => await existsElement.ExistsById<CertifiedEntity>(s));

                    if (existsCertifiedsId.Any(s => !s.Result))
                    {
                        return "Uno o más de los id de Entidad certificadora no existe";
                    }
                }
            }

            
            

            return string.Empty;
        }
    }
}
