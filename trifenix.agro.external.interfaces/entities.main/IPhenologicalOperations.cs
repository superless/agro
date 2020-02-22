using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IPhenologicalOperations
    {
        Task<ExtPostContainer<string>> SaveNewPhenologicalEvent(string name, DateTime startDate, DateTime endDate);

        Task<ExtPostContainer<PhenologicalEvent>> SaveEditPhenologicalEvent(string currentId, string name, DateTime startDate, DateTime endDate);

        Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents();
    }
}
