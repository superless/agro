using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class NebulizerOperations : INebulizerOperations
    {
        private readonly INebulizerRepository _repo;
        private readonly ICommonDbOperations<Nebulizer> _commonDb;
        public NebulizerOperations(INebulizerRepository repo, ICommonDbOperations<Nebulizer> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<Nebulizer>>> GetNebulizers()
        {
            var queryTargets = _repo.GetNebulizers();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<Nebulizer>> SaveEditNebulizer(string id, string brand, string code)
        {
            var element = await _repo.GetNebulizer(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetNebulizers(),
                id,
                element,
                s => {
                    s.Brand = brand;
                    s.Code = code;
                    return s;
                },
                _repo.CreateUpdateNebulizer,
                 $"No existe objetivo aplicación con id: {id}",
                s => s.Code.Equals(code) && code != element.Code,
                $"Ya existe nebulizadora con codigo: {code}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewNebulizer(string brand, string code)
        {
            return await OperationHelper.CreateElement(_commonDb,_repo.GetNebulizers(),
                async s => await _repo.CreateUpdateNebulizer(new Nebulizer
                {
                    Id = s,
                    Brand = brand,
                    Code = code
                }),
                s => s.Code.Equals(code),
                $"Ya existe nebulizadora con codigo: {code}"
            );
        }
    }
}
