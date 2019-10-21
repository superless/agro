using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Field;
using trifenix.agro.db.model.enforcements;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.applicationsReference.Field
{
    public class AgroVarietyContainer : MainDb<AgroVariety>, IAgroVarietyContainer
    {
        public AgroVarietyContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateVariety(AgroVariety variety)
        {
            return await CreateUpdate(variety);
        }

        public IQueryable<AgroVariety> GetVarieties()
        {
            return GetEntities();
        }

        public Task<AgroVariety> GetVariety(string uniqueId)
        {
            return GetEntity(uniqueId);
        }
    }
}
