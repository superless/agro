using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.applicationOrders;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.applicationsReference.ApplicationOrders
{
    public class RefAppContinuedContainer : MainDb<RefApplicationContinued>, IRefAppContinuedContainer
    {
        
        public RefAppContinuedContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {
        
        }

        public async Task<string> CreateUpdateContinuedAppRef(RefApplicationContinued continuedApp)
        {
            return await CreateUpdate(continuedApp);
        }

        public async Task<RefApplicationContinued> GetContinuedAppRef(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<RefApplicationContinued> GetContinuedAppRefs()
        {
            return GetEntities();
        }
    }
}
