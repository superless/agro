using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IOrderFolderRepository
    {
        Task<string> CreateUpdateOrderFolder(OrderFolder orderFolder);

        Task<OrderFolder> GetOrderFolder(string id);

        IQueryable<OrderFolder> GetOrderFolders();

    }
}
