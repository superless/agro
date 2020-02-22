using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro.orders
{
    public class OrderFolderRepository : IOrderFolderRepository
    {

        private readonly IMainDb<OrderFolder> _db;
        public OrderFolderRepository(IMainDb<OrderFolder> db) 
        {
            _db = db;
        }


        public async Task<string> CreateUpdateOrderFolder(OrderFolder orderFolder)
        {
            return await _db.CreateUpdate(orderFolder);
        }

        public async Task<OrderFolder> GetOrderFolder(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<OrderFolder> GetOrderFolders()
        {
            return _db.GetEntities();
        }
    }
}
