using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.ext
{
    public interface ICertifiedEntityRepository
    {
        Task<string> CreateUpdateCertifiedEntity(CertifiedEntity product);

        Task<CertifiedEntity> GetCertifiedEntity(string id);

        IQueryable<CertifiedEntity> GetCertifiedEntities();
    }


}
