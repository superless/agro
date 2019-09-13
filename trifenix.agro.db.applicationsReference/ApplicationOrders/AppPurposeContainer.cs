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
    public class AppPurposeContainer : MainDb<ApplicationPurpose>, IAppPurposeContainter
    {
        private readonly ICosmosStore<ApplicationPurpose> appPurposeStore;


        public AppPurposeContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {
            
        }


        public async Task<string> CreateUpdateApplicationPurpose(ApplicationPurpose appPurpose)
        {
            return await CreateUpdate(appPurpose);
        }

        public async Task<ApplicationPurpose> GetApplicationPurpose(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<ApplicationPurpose> GetApplicationPurposes()
        {
            return GetEntities();
        }
    }
}
