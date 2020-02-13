using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.core {

    public interface IBusinessNameOperations {
        Task<ExtPostContainer<string>> SaveNewBusinessName(string name, string rut, string phone, string email, string webPage, string giro);
        Task<ExtPostContainer<BusinessName>> SaveEditBusinessName(string idBusinessName, string name, string rut, string phone, string email, string webPage, string giro);
        Task<ExtGetContainer<BusinessName>> GetBusinessName(string id);
        Task<ExtGetContainer<List<BusinessName>>> GetBusinessNames();
        
    }

}