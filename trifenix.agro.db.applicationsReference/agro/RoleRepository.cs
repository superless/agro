using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class RoleRepository : IRoleRepository
    {

        private readonly IMainDb<Role> _db;
        public RoleRepository(IMainDb<Role> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateRole(Role Role)
        {
            return await _db.CreateUpdate(Role);
        }

        public async Task<Role> GetRole(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Role> GetRoles()
        {
            return _db.GetEntities();
        }
    }
}
