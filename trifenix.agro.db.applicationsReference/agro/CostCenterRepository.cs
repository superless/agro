using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.applicationsReference.agro {
    public class CostCenterRepository : MainGenericDb<CostCenter>, IMainGenericDb<CostCenter>
    {
        public CostCenterRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}