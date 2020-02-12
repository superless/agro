using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.core {

    public interface ICostCenterOperations {
        Task<ExtPostContainer<string>> SaveNewCostCenter(string name, string idRazonSocial);
        Task<ExtPostContainer<CostCenter>> SaveEditCostCenter(string idCostCenter, string name, string idRazonSocial);
        Task<ExtGetContainer<CostCenter>> GetCostCenter(string id);
        Task<ExtGetContainer<List<CostCenter>>> GetCostCenters();
    }

}