using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.orders
{
    public class OrderFolderRepository : MainDb<OrderFolder>, IOrderFolderRepository
    {
        public OrderFolderRepository(AgroDbArguments args) : base(args)
        {
        }


        public async Task<string> CreateUpdateOrderFolder(OrderFolder orderFolder)
        {
            return await CreateUpdate(orderFolder);
        }

        public async Task<OrderFolder> GetOrderFolder(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<OrderFolder> GetOrderFolders()
        {
            return GetEntities();
        }
    }
}
