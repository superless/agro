using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.applicationOrders;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.applicationsReference.ApplicationOrders
{
    public class RefAppDateContainer : MainDb<RefApplicationDate>, IRefAppDateContainer
    {

        public RefAppDateContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {

        }

        public async Task<string> CreateUpdateDateAppRef(RefApplicationDate dateApp)
        {
            return await CreateUpdate(dateApp);
        }

        public  IQueryable<RefApplicationDate> GetDateAppRefs()
        {
            return GetEntities();
        }

        public async Task<RefApplicationDate> GeteDateAppRef(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }
    }
}
