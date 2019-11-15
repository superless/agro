

using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.ext
{
    public interface ICertifiedEntityOperations
    {

        Task<ExtPostContainer<string>> SaveNewCertifiedEntity(string name, string abbreviation);

        Task<ExtPostContainer<CertifiedEntity>> SaveEditCertifiedEntity(string id, string name, string abbreviation);

        Task<ExtGetContainer<List<CertifiedEntity>>> GetCertifiedEntities();

        Task<ExtGetContainer<CertifiedEntity>> GetCertifiedEntity(string id);



    }
}
