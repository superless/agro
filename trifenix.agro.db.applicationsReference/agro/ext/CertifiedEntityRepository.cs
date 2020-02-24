using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.ext
{
    public class CertifiedEntityRepository : MainGenericDb<CertifiedEntity>, IMainGenericDb<CertifiedEntity>
    {
        public CertifiedEntityRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
