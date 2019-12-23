using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface INebulizerOperations
    {
        Task<ExtPostContainer<string>> SaveNewNebulizer(string brand, string code);

        Task<ExtPostContainer<Nebulizer>> SaveEditNebulizer(string id, string brand, string code);

        Task<ExtGetContainer<List<Nebulizer>>> GetNebulizers();

    }
}
