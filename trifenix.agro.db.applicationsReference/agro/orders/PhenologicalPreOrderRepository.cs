using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.orders
{
    public class PhenologicalPreOrderRepository : IPhenologicalPreOrderRepository
    {

        private readonly IMainDb<PhenologicalPreOrder> _db;
        public PhenologicalPreOrderRepository(AgroDbArguments dbArguments) 
        {
            _db = new MainDb<PhenologicalPreOrder>(dbArguments);
        }

        public async Task<string> CreateUpdatePhenologicalPreOrder(PhenologicalPreOrder preOrder)
        {
            return await _db.CreateUpdate(preOrder);
        }

        public async Task<PhenologicalPreOrder> GetPhenologicalPreOrder(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<PhenologicalPreOrder> GetPhenologicalPreOrders()
        {
            return _db.GetEntities();
        }
    }
}
