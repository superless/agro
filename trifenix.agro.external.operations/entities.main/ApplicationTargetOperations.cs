using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{


    public class ApplicationTargetOperations : IApplicationTargetOperations
    {
        private readonly IApplicationTargetRepository _repo;
        public ApplicationTargetOperations(IApplicationTargetRepository repo)
        {
            _repo = repo;
        }
        public async Task<ExtGetContainer<List<ApplicationTarget>>> GetAplicationsTarget()
        {
            var elements = await _repo.GetTargets().ToListAsync();
            return OperationHelper.GetElements(elements);
        }

        public async Task<ExtPostContainer<ApplicationTarget>> SaveEditApplicationTarget(string id, string name)
        {
            var element = await _repo.GetTarget(id);
            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateTargetApp,
                 $"No existe objetivo aplicación con id : {id}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewApplicationTarget(string name)
        {
            return await OperationHelper.CreateElement(_repo.GetTargets(),
                async s => await _repo.CreateUpdateTargetApp(new ApplicationTarget
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"ya existe especie con nombre {name} "

            );
        }
    }
}
