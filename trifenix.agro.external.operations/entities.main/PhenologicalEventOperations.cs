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
    public class PhenologicalEventOperations : IPhenologicalOperations
    {
        private readonly IPhenologicalEventRepository _repo;

        public PhenologicalEventOperations(IPhenologicalEventRepository repo)
        {
            _repo = repo;
        }


        public async Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents()
        {
            var elements = await _repo.GetPhenologicalEvents().ToListAsync();
            return OperationHelper.GetElements(elements);
        }

        public async Task<ExtPostContainer<PhenologicalEvent>> SaveEditPhenologicalEvent(string currentId, string name, DateTime startDate, DateTime endDate)
        {
            var element = await _repo.GetPhenologicalEvent(currentId);
            return await OperationHelper.EditElement(currentId,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdatePhenologicalEvent,
                 $"No existe especie con id : {currentId}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewPhenologicalEvent(string name, DateTime startDate, DateTime endDate)
        {
            return await OperationHelper.CreateElement(_repo.GetPhenologicalEvents(),
                async s => await _repo.CreateUpdatePhenologicalEvent(new PhenologicalEvent {
                    Id = s,
                    Name = name,
                    InitDate = startDate,
                    EndDate = endDate
                }),
                s => s.Name.Equals(name),
                $"ya existe Evento fenológico con nombre {name} "

            );
        }

        
    }
}
