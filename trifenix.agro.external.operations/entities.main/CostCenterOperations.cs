using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.external.interfaces.entities.core;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main {
    public class CostCenterOperations : ICostCenterOperations {

        private readonly ICostCenterRepository _repo;
        private readonly IBusinessNameRepository _repoBusinessName;
        private readonly ICommonDbOperations<CostCenter> _commonDb;
        private readonly IGraphApi _graphApi;
        public CostCenterOperations(ICostCenterRepository repo, IBusinessNameRepository repoBusinessName, ICommonDbOperations<CostCenter> commonDb, IGraphApi graphApi) {
            _repo = repo;
            _repoBusinessName = repoBusinessName;
            _commonDb = commonDb;
            _graphApi = graphApi;
        }
        public async Task<ExtPostContainer<string>> SaveNewCostCenter(string name, string idRazonSocial) {
            BusinessName businessName = await _repoBusinessName.GetBusinessName(idRazonSocial);
            if (businessName == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró la razon social con id {idRazonSocial}", idRazonSocial);
            UserApplicator modifier = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, modifier); 
            return await OperationHelper.CreateElement(_commonDb, _repo.GetCostCenters(),
                async s => await _repo.CreateUpdateCostCenter(new CostCenter {
                    Id = s,
                    Name = name,
                    IdRazonSocial = idRazonSocial,
                    Modify = userActivity
                }),
                s => s.Name.Equals(name),
                $"Ya existe centro de costo  con nombre: {name}"
            );
        }

        public async Task<ExtPostContainer<CostCenter>> SaveEditCostCenter(string idCostCenter, string name, string idRazonSocial) {
            CostCenter costCenter = await _repo.GetCostCenter(idCostCenter);
            if (costCenter == null)
                return OperationHelper.PostNotFoundElementException<CostCenter>($"No se encontró el centro de costo con id {idCostCenter}", idCostCenter);
            BusinessName businessName = await _repoBusinessName.GetBusinessName(idRazonSocial);
            if (businessName == null)
                return OperationHelper.PostNotFoundElementException<CostCenter>($"No se encontró la razon social con id {idRazonSocial}", idRazonSocial);
            UserApplicator modifier = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, modifier);
            return await OperationHelper.EditElement(_commonDb, _repo.GetCostCenters(),
                idCostCenter,
                costCenter,
                s => {
                    s.Name = name;
                    s.IdRazonSocial = idRazonSocial;
                    s.Modify = userActivity;
                    return s;
                },
                _repo.CreateUpdateCostCenter,
                 $"No existe objetivo aplicación con id: {idCostCenter}",
                s => s.Name.Equals(name) && name != costCenter.Name,
                $"Ya existe centro de costo con nombre: {name}"
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
