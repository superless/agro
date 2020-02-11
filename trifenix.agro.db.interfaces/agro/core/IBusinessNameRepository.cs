using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.interfaces.agro.core
{
    public interface IBusinessNameRepository
    {
        Task<string> CreateUpdateBusinessName(BusinessName businessName);

        Task<BusinessName> GetBusinessName(string id);

        IQueryable<BusinessName> GetBusinessNames();



        
    }
}
