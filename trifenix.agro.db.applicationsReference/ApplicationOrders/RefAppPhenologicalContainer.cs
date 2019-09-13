using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.applicationOrders;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.applicationsReference.ApplicationOrders
{
    public class RefAppPhenologicalContainer : MainDb<RefApplicaByPhenologicalEvent>, IRefAppPhenologicalContainer
    {

        public RefAppPhenologicalContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {

        }
        public async Task<string> CreateUpdatePhenologicalAppRef(RefApplicaByPhenologicalEvent phenologicalApp)
        {
            return await CreateUpdate(phenologicalApp);
        }

        public async Task<RefApplicaByPhenologicalEvent> GetPhenologicalAppRef(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<RefApplicaByPhenologicalEvent> GetPhenologicalAppRefs()
        {
            return GetEntities();
        }
    }
}
