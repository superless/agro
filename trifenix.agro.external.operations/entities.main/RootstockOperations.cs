using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class RootstockOperations : IRootstockOperations
    {
        private readonly IRootstockRepository _repo;
        private readonly ICommonDbOperations<Rootstock> _commonDb;
        public RootstockOperations(IRootstockRepository repo, ICommonDbOperations<Rootstock> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<Rootstock>>> GetRootstocks()
        {
            var rootstocksQuery = _repo.GetRootstocks();
            var rootstocks = await _commonDb.TolistAsync(rootstocksQuery);
            return OperationHelper.GetElements(rootstocks);

        }

        public async Task<ExtPostContainer<Rootstock>> SaveEditRootstock(string id, string name, string abbreviation)
        {
            var element = await _repo.GetRootstock(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetRootstocks(),
                id, 
                element, 
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    return s;
                },
                _repo.CreateUpdateRootstock,
                 $"No existe portainjerto con id : {id}",
                s => s.Name.Equals(name),
                $"Ya existe portainjerto con nombre: {name}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewRootstock(string name, string abbreviation)
        {
            
            return await OperationHelper.CreateElement(_commonDb, _repo.GetRootstocks(), 
                async s => await _repo.CreateUpdateRootstock(new Rootstock
                {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation
                }),
                s => s.Name.Equals(name),
                $"Ya existe portainjerto con nombre: {name}"
            );
            
        }
    }
}
