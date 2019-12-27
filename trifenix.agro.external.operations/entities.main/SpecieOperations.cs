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
    public class SpecieOperations : ISpecieOperations
    {
        private readonly ISpecieRepository _repo;
        private readonly ICommonDbOperations<Specie> _commonDb;
        public SpecieOperations(ISpecieRepository repo, ICommonDbOperations<Specie> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<Specie>>> GetSpecies()
        {
            var speciesQuery = _repo.GetSpecies();
            var species = await _commonDb.TolistAsync(speciesQuery);
            return OperationHelper.GetElements(species);

        }

        public async Task<ExtPostContainer<Specie>> SaveEditSpecie(string id, string name, string abbreviation)
        {
            var element = await _repo.GetSpecie(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetSpecies(), 
                id, 
                element, 
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    return s;
                },
                _repo.CreateUpdateSpecie,
                 $"No existe especie con id : {id}",
                s => s.Name.Equals(name),
                $"Ya existe especie con nombre: {name}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewSpecie(string name, string abbreviation)
        {
            
            return await OperationHelper.CreateElement(_commonDb, _repo.GetSpecies(), 
                async s => await _repo.CreateUpdateSpecie(new Specie
                {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation
                }),
                s => s.Name.Equals(name),
                $"Ya existe especie con nombre: {name}"

            );
            
        }
    }
}
