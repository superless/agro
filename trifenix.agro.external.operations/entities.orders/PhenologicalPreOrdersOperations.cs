using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.orders
{
    public class PhenologicalPreOrders : IPhenologicalPreOrderOperations
    {

        private readonly IPhenologicalPreOrderRepository _repo;
        private readonly string idSeason;
        public PhenologicalPreOrders()
        {

        }

        public Task<ExtGetContainer<PhenologicalPreOrder>> GetPhenologicalPreOrder(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ExtGetContainer<List<PhenologicalPreOrder>>> GetPhenologicalPreOrders()
        {
            throw new NotImplementedException();
        }

        public Task<ExtPostContainer<PhenologicalPreOrder>> SaveEditPhenologicalPreOrder(string id, string idSeason, string name, string idOrderFolder, List<string> idBarracks)
        {
            throw new NotImplementedException();
        }

        public Task<ExtPostContainer<string>> SaveNewPhenologicalPreOrder(string name, string idOrderFolder, List<string> idBarracks)
        {
            throw new NotImplementedException();
        }
    }
}
