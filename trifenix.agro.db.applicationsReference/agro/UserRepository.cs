using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro {
    public class UserRepository : MainGenericDb<UserApplicator>, IMainGenericDb<UserApplicator>
    {
        public UserRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
