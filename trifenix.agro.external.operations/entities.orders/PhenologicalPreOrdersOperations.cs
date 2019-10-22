using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.orders
{
    public class PhenologicalPreOrdersOperations : IPhenologicalPreOrderOperations
    {

        private readonly IPhenologicalPreOrderRepository _repo;
        private readonly string _idSeason;
        public PhenologicalPreOrdersOperations(IPhenologicalPreOrderRepository repo, string idSeason)
        {
            _repo = repo;
            _idSeason = idSeason;

        }

        public async Task<ExtGetContainer<PhenologicalPreOrder>> GetPhenologicalPreOrder(string id)
        {
            var preorder = await _repo.GetPhenologicalPreOrder(id);
            return OperationHelper.GetElement(preorder);
        }

        public async Task<ExtGetContainer<List<PhenologicalPreOrder>>> GetPhenologicalPreOrders()
        {
            var preorders = await _repo.GetPhenologicalPreOrders().ToListAsync();
            return OperationHelper.GetElements(preorders);
        }

        public async Task<ExtPostContainer<PhenologicalPreOrder>> SaveEditPhenologicalPreOrder(string id, string name, string idOrderFolder, List<string> idBarracks)
        {
            var element = await _repo.GetPhenologicalPreOrder(id);

            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    s.SeasonId = _idSeason;
                    s.BarracksId = idBarracks;
                    s.OrderFolderId = idOrderFolder;
                    s.Created = DateTime.Now;
                    return s;
                },
                _repo.CreateUpdatePhenologicalPreOrder,
                 $"No existe PreOrden Fenológica con id : {id}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewPhenologicalPreOrder(string name, string idOrderFolder, List<string> idBarracks)
        {
            return await OperationHelper.CreateElement(_repo.GetPhenologicalPreOrders(),
               async s => await _repo.CreateUpdatePhenologicalPreOrder(new PhenologicalPreOrder
               {
                   Id = s,
                   Name = name,
                   SeasonId = _idSeason,
                   BarracksId = idBarracks,
                   Created = DateTime.Now,
                   OrderFolderId = idOrderFolder
               }),
               s => s.Name.Equals(name),
               $"ya existe Cuartel con nombre {name} "

           ); 
        }
    }
}
