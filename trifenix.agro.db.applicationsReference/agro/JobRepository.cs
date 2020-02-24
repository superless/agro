using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class JobRepository : MainGenericDb<Job>, IMainGenericDb<Job>
    {
        public JobRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
