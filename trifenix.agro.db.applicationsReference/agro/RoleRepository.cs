using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class RoleRepository : MainGenericDb<Role>, IMainGenericDb<Role>
    {
        public RoleRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
