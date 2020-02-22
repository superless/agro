using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro.ext
{
    public class CertifiedEntityRepository : ICertifiedEntityRepository
    {

        private readonly IMainDb<CertifiedEntity> _db;
        public CertifiedEntityRepository(IMainDb<CertifiedEntity> db)
        {
            _db = db;
        }



        public async Task<string> CreateUpdateCertifiedEntity(CertifiedEntity product)
        {
            return await _db.CreateUpdate(product);
        }

        public IQueryable<CertifiedEntity> GetCertifiedEntities()
        {
            return _db.GetEntities();
        }

        public async Task<CertifiedEntity> GetCertifiedEntity(string id)
        {
            return await _db.GetEntity(id);
        }


    }
}
