using trifenix.agro.db.interfaces;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SpecieRepository : MainGenericDb<Specie>, IMainGenericDb<Specie>
    {
        public SpecieRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
