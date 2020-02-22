using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main {
    public interface ITractorOperations {
        Task<ExtPostContainer<string>> SaveNewTractor(string brand, string code);

        Task<ExtPostContainer<Tractor>> SaveEditTractor(string id, string brand, string code);

        Task<ExtGetContainer<List<Tractor>>> GetTractors();

        Task<ExtGetContainer<Tractor>> GetTractor(string idTractor);

    }
}