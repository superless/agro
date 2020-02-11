using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.core;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.applicationsReference.agro.core
{
    public class BusinessNameRepository : IBusinessNameRepository
    {

        private readonly IMainDb<BusinessName> _db;


        public async Task<string> CreateUpdateBusinessName(BusinessName businessName)
        {
            return await _db.CreateUpdate(businessName);
        }

        public async Task<BusinessName> GetBusinessName(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<BusinessName> GetBusinessNames()
        {
            return _db.GetEntities();
        }
    }
}
