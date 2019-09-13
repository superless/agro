using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.interfaces.Field
{
    public interface IAgroVarietyContainer
    {
        Task<string> CreateUpdateVariety(AgroVariety variety);
        Task<AgroVariety> GetVariety(string uniqueId);
        IQueryable<AgroVariety> GetVarieties();
        
    }
}
