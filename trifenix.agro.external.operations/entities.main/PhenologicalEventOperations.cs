using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class PhenologicalEventOperations : IPhenologicalOperations
    {
        private readonly IPhenologicalEventRepository _repo;
        private readonly ICommonDbOperations<PhenologicalEvent> _commonDb;

        public PhenologicalEventOperations(IPhenologicalEventRepository repo, ICommonDbOperations<PhenologicalEvent> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents()
        {
            var phenologicalsQuery = _repo.GetPhenologicalEvents();
            var phenologicalEvents = await _commonDb.TolistAsync(phenologicalsQuery);
            return OperationHelper.GetElements(phenologicalEvents);
        }

        public async Task<ExtPostContainer<PhenologicalEvent>> SaveEditPhenologicalEvent(string currentId, string name, DateTime startDate, DateTime endDate)
        {
            var element = await _repo.GetPhenologicalEvent(currentId);
            return await OperationHelper.EditElement(_commonDb, _repo.GetPhenologicalEvents(),
                currentId,
                element,
                s => {
                    s.Name = name;
                    s.InitDate = startDate;
                    s.EndDate = endDate;
                    return s;
                },
                _repo.CreateUpdatePhenologicalEvent,
                 $"No existe especie con id : {currentId}",
                s => s.Name.Equals(name) && name != element.Name,
                $"Ya existe Evento fenológico con nombre: {name}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewPhenologicalEvent(string name, DateTime startDate, DateTime endDate)
        {
            return await OperationHelper.CreateElement(_commonDb, _repo.GetPhenologicalEvents(),
                async s => await _repo.CreateUpdatePhenologicalEvent(new PhenologicalEvent {
                    Id = s,
                    Name = name,
                    InitDate = startDate,
                    EndDate = endDate
                }),
                s => s.Name.Equals(name),
                $"Ya existe Evento fenológico con nombre: {name}"

            );
        }

    }
}
