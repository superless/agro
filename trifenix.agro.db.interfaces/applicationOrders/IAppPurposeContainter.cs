using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.interfaces.applicationOrders
{
    public interface IAppPurposeContainter
    {
        Task<string> CreateUpdateApplicationPurpose(ApplicationPurpose appPurpose);

        Task<ApplicationPurpose> GetApplicationPurpose(string uniqueId);


        IQueryable<ApplicationPurpose> GetApplicationPurposes();

    }
}
