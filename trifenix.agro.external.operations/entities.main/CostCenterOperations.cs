using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.external.interfaces.entities.core;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main {
    public class CostCenterOperations : ICostCenterOperations {

        private readonly ICostCenterRepository _repo;
        private readonly ICommonDbOperations<CostCenter> _commonDb;
        public CostCenterOperations(ICostCenterRepository repo, ICommonDbOperations<CostCenter> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtPostContainer<string>> SaveNewCostCenter(string name, string rut, string phone, string email, string webPage, string giro)
        {
            return await OperationHelper.CreateElement(_commonDb, _repo.GetCostCenters(),
                async s => await _repo.CreateUpdateCostCenter(new CostCenter
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"Ya existe rol con nombre: {name}"
            );
        }

        public async Task<ExtPostContainer<CostCenter>> SaveEditCostCenter(string idCostCenter, string name, string rut, string phone, string email, string webPage, string giro)
        {
            var costCenter = await _repo.GetCostCenter(idCostCenter);
            return await OperationHelper.EditElement(_commonDb, _repo.GetCostCenters(),
                idCostCenter,
                costCenter,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateCostCenter,
                 $"No existe objetivo aplicación con id: {idCostCenter}",
                s => s.Name.Equals(name) && name != costCenter.Name,
                $"Ya existe rol con nombre: {name}"
            );

        }
        public async Task<ExtGetContainer<CostCenter>> GetCostCenter(string idCostCenter) {
            var costCenter = await _repo.GetCostCenter(idCostCenter);
            return OperationHelper.GetElement(costCenter);
        }

        public async Task<ExtGetContainer<List<CostCenter>>> GetCostCenters() {
            var queryTargets = _repo.GetCostCenters();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

    }
}
