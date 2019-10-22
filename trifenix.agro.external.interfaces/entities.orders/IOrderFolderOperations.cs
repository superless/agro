using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IOrderFolderOperations
    {
        Task<ExtPostContainer<string>> SaveNewOrderFolder(string idPhenologicalEvent, string idApplicationTarget, string categoryId, string idSpecie, string idIngredient);

        Task<ExtPostContainer<OrderFolder>> SaveEditOrderFolder(string id, string idPhenologicalEvent, string idApplicationTarget, string categoryId, string idSpecie, string idIngredient);

        Task<ExtGetContainer<List<OrderFolder>>> GetOrderFolders();

        Task<ExtGetContainer<OrderFolder>> GetOrderFolder(string id);
    }
}
