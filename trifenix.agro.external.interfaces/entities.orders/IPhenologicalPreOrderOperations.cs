using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IPhenologicalPreOrderOperations
    {
        Task<ExtPostContainer<string>> SaveNewPhenologicalPreOrder(string name, string idOrderFolder, List<string> idBarracks);

        Task<ExtPostContainer<PhenologicalPreOrder>> SaveEditPhenologicalPreOrder(string id, string idSeason, string name, string idOrderFolder, List<string> idBarracks);


        Task<ExtGetContainer<List<PhenologicalPreOrder>>> GetPhenologicalPreOrders();


        Task<ExtGetContainer<PhenologicalPreOrder>> GetPhenologicalPreOrder(string id);
    }
}
