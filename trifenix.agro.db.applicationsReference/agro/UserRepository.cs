using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class UserRepository : IUserRepository
    {

        private readonly IMainDb<UserApplicator> _db;
        public UserRepository(IMainDb<UserApplicator> db)
        {
            _db = db;
        }

        public async Task<string> CreateUpdateUser(UserApplicator User)
        {
            return await _db.CreateUpdate(User);
        }

        public IQueryable<UserApplicator> GetUsers()
        {
            return _db.GetEntities();
        }

        public async Task<UserApplicator> GetUser(string id)
        {
            return await _db.GetEntity(id);
        }
    }
}
