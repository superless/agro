using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class SpecieOperations : ISpecieOperations
    {
        private readonly ISpecieRepository _repo;

        public SpecieOperations(ISpecieRepository repo)
        {
            _repo = repo;
        }

        public async Task<ExtGetContainer<List<Specie>>> GetSpecies()
        {
            var elements = await _repo.GetSpecies().ToListAsync();
            return OperationHelper.GetElements(elements);

        }

        public async Task<ExtPostContainer<Specie>> SaveEditSpecie(string id, string name, string abbreviation)
        {
            var element = await _repo.GetSpecie(id);
            return await OperationHelper.EditElement(id, 
                element, 
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    return s;
                },
                _repo.CreateUpdateSpecie,
                 $"No existe especie con id : {id}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewSpecie(string name, string abbreviation)
        {
            
            return await OperationHelper.CreateElement(_repo.GetSpecies(), 
                async s => await _repo.CreateUpdateSpecie(new Specie
                {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation
                }),
                s => s.Name.Equals(name),
                $"ya existe especie con nombre {name} "

            );
            
        }
    }
}
