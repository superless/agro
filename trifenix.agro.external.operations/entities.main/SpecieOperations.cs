using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main {
    public class SpecieOperations : ISpecieOperations {

        private readonly ISpecieRepository _repo;
        private readonly ICommonDbOperations<Specie> _commonDb;

        public SpecieOperations(ISpecieRepository repo, ICommonDbOperations<Specie> commonDb) {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<Specie>> GetSpecie(string id) {
            var specie = await _repo.GetSpecie(id);
            return OperationHelper.GetElement(specie);
        }

        public async Task<ExtGetContainer<List<Specie>>> GetSpecies() {
            var speciesQuery = _repo.GetSpecies();
            var species = await _commonDb.TolistAsync(speciesQuery);
            return OperationHelper.GetElements(species);
        }

        public async Task<ExtPostContainer<Specie>> SaveEditSpecie(string idSpecie, string name, string abbreviation) {
            if (string.IsNullOrWhiteSpace(idSpecie)) return OperationHelper.GetPostException<Specie>(new Exception("Es requerido 'idSpecie'."));
            if (string.IsNullOrWhiteSpace(name)) return OperationHelper.GetPostException<Specie>(new Exception("Es requerido 'name'."));
            if (string.IsNullOrWhiteSpace(abbreviation)) return OperationHelper.GetPostException<Specie>(new Exception("Es requerido 'abbreviation'."));
            Specie specie = await _repo.GetSpecie(idSpecie);
            if (specie == null)
                return OperationHelper.PostNotFoundElementException<Specie>($"No se encontró la especie con id {idSpecie}", idSpecie);
            return await OperationHelper.EditElement(_commonDb, _repo.GetSpecies(), 
                idSpecie, 
                specie, 
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    return s;
                },
                _repo.CreateUpdateSpecie,
                 $"No existe especie con id : {idSpecie}",
                s => (s.Name.Equals(name) && !specie.Name.Equals(name)) || (s.Abbreviation.Equals(abbreviation) && !specie.Abbreviation.Equals(abbreviation)),
                $"Ya existe especie con nombre: {name} o abreviación: {abbreviation}. Ambos campos deben ser unicos."
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewSpecie(string name, string abbreviation) {
            if (string.IsNullOrWhiteSpace(name)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'name' para crear una especie."));
            if (string.IsNullOrWhiteSpace(abbreviation)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'abbreviation' para crear una especie."));
            return await OperationHelper.CreateElement(_commonDb, _repo.GetSpecies(), 
                async s => await _repo.CreateUpdateSpecie(new Specie {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation
                }),
                s => s.Name.Equals(name) || s.Abbreviation.Equals(abbreviation),
                $"Ya existe especie con nombre: {name} o abreviación: {abbreviation}. Ambos campos deben ser unicos."
            );
        }

    }
}
