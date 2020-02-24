using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class PlotLandRepository : MainGenericDb<PlotLand>, IMainGenericDb<PlotLand>
    {
        public PlotLandRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
