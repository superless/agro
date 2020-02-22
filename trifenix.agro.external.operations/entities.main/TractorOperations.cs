using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main {
    public class TractorOperations : ITractorOperations {

        private readonly ITractorRepository _repo;
        private readonly ICommonDbOperations<Tractor> _commonDb;

        public TractorOperations(ITractorRepository repo, ICommonDbOperations<Tractor> commonDb) {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<Tractor>> GetTractor(string idTractor) {
            var tractor = await _repo.GetTractor(idTractor);
            return OperationHelper.GetElement(tractor);
        }

        public async Task<ExtGetContainer<List<Tractor>>> GetTractors() {
            var queryTargets = _repo.GetTractors();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<Tractor>> SaveEditTractor(string id, string brand, string code) {
            var element = await _repo.GetTractor(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetTractors(),
                id,
                element,
                s => {
                    s.Brand = brand;
                    s.Code = code;
                    return s;
                },
                _repo.CreateUpdateTractor,
                $"No existe objetivo aplicación con id : {id}",
                s => s.Code.Equals(code) && code != element.Code,
                $"Ya existe tractor con codigo {code}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewTractor(string brand, string code) {
            return await OperationHelper.CreateElement(_commonDb,_repo.GetTractors(),
                async s => await _repo.CreateUpdateTractor(new Tractor {
                    Id = s,
                    Brand = brand,
                    Code = code
                }),
                s => s.Code.Equals(code),
                $"Ya existe tractor con codigo {code}"
            );
        }

    }
}