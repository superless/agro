using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class VarietyRepository : MainDb<Variety>, IVarietyRepository
    {
        public VarietyRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateVariety(Variety variety)
        {
            return await CreateUpdate(variety);
        }

        public IQueryable<Variety> GetVarieties()
        {
            return GetEntities();
        }

        public async Task<Variety> GetVariety(string id)
        {
            return await GetEntity(id);
        }
    }
}
