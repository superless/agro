using System;
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


    public class ApplicationTargetOperations : IApplicationTargetOperations
    {
        private readonly IApplicationTargetRepository _repo;
        private readonly ICommonDbOperations<ApplicationTarget> _commonDb;
        public ApplicationTargetOperations(IApplicationTargetRepository repo, ICommonDbOperations<ApplicationTarget> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<ApplicationTarget>>> GetAplicationsTarget()
        {
            var queryTargets = _repo.GetTargets();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<ApplicationTarget>> SaveEditApplicationTarget(string id, string name)
        {
            var element = await _repo.GetTarget(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetTargets(),
                id,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateTargetApp,
                 $"No existe objetivo aplicación con id: {id}",
                s => s.Name.Equals(name),
                $"Ya existe aplicacion target con nombre: {name}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewApplicationTarget(string name)
        {
            return await OperationHelper.CreateElement(_commonDb,_repo.GetTargets(),
                async s => await _repo.CreateUpdateTargetApp(new ApplicationTarget
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"Ya existe aplicacion target con nombre: {name}"

            );
        }
    }
}
