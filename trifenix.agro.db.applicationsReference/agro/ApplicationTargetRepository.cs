using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class ApplicationTargetRepository : MainDb<ApplicationTarget>, IApplicationTargetRepository
    {
        public ApplicationTargetRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateTargetApp(ApplicationTarget target)
        {
            return await CreateUpdate(target);
        }

        public async Task<ApplicationTarget> GetTarget(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<ApplicationTarget> GetTargets()
        {
            return GetEntities();
        }
    }
}
