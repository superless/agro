using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IVarietyRepository
    {
        Task<string> CreateUpdateVariety(Variety variety);

        Task<Variety> GetVariety(string id);

        IQueryable<Variety> GetVarieties();

    }
}
