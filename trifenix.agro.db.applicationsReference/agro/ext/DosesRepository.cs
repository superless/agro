using trifenix.agro.db.interfaces;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.ext
{
    public class DosesRepository : MainGenericDb<Doses>, IMainGenericDb<Doses>
    {
        public DosesRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
