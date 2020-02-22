using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class VarietyOperations : IVarietyOperations
    {

        private readonly IVarietyRepository _repo;
        private readonly ISpecieRepository _repoSpecie;
        private readonly ICommonDbOperations<Variety> _commonDb;
        public VarietyOperations(IVarietyRepository repo, ISpecieRepository repoSpecie, ICommonDbOperations<Variety> commonDb)
        {
            _repo = repo;
            _repoSpecie = repoSpecie;
            _commonDb = commonDb;
        }


        public async Task<ExtGetContainer<List<Variety>>> GetVarieties()
        {
            var varietiesQuery = _repo.GetVarieties();
            var varieties = await _commonDb.TolistAsync(varietiesQuery);
            return OperationHelper.GetElements(varieties);
        }

        public async Task<ExtGetContainer<Variety>> GetVariety(string id) {
            var variety = await _repo.GetVariety(id);
            return OperationHelper.GetElement(variety);
        }

        public async Task<ExtPostContainer<Variety>> SaveEditVariety(string idVariety, string name, string abbreviation, string idSpecie) {
            if (string.IsNullOrWhiteSpace(idVariety)) return OperationHelper.GetPostException<Variety>(new Exception("Es requerido 'idVariety' para crear una variedad."));
            if (string.IsNullOrWhiteSpace(name)) return OperationHelper.GetPostException<Variety>(new Exception("Es requerido 'name' para crear una variedad."));
            if (string.IsNullOrWhiteSpace(abbreviation)) return OperationHelper.GetPostException<Variety>(new Exception("Es requerido 'abbreviation' para crear una variedad."));
            if (string.IsNullOrWhiteSpace(idSpecie)) return OperationHelper.GetPostException<Variety>(new Exception("Es requerido 'idSpecie' para crear una variedad."));
            Variety variety = await _repo.GetVariety(idVariety);
            if (variety == null)
                return OperationHelper.PostNotFoundElementException<Variety>($"No se encontró la variedad con id {idVariety}", idVariety);
            Specie specie = await _repoSpecie.GetSpecie(idSpecie);
            if (specie == null)
                return OperationHelper.PostNotFoundElementException<Variety>($"No se encontró la especie con id {idSpecie}", idSpecie);
            return await OperationHelper.EditElement(_commonDb, _repo.GetVarieties(), 
                idVariety,
                variety,
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    s.Specie = specie;
                    return s;
                },
                _repo.CreateUpdateVariety,
                 $"No existe variedad con id: {idVariety}",
                s => (s.Name.Equals(name) && variety.Name != name) || (s.Abbreviation.Equals(abbreviation) && variety.Abbreviation != abbreviation),
                $"Ya existe variedad con nombre: {name} o abreviación: {abbreviation}. Ambos campos deben ser unicos."
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewVariety(string name, string abbreviation, string idSpecie) {
            if (string.IsNullOrWhiteSpace(name)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'name' para crear una variedad."));
            if (string.IsNullOrWhiteSpace(abbreviation)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'abbreviation' para crear una variedad."));
            if (string.IsNullOrWhiteSpace(idSpecie)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'idSpecie' para crear una variedad."));
            Specie specie = await _repoSpecie.GetSpecie(idSpecie);
            if (specie == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró la especie con id {idSpecie}", idSpecie);
            return await OperationHelper.CreateElement(_commonDb, _repo.GetVarieties(),
                async s => await _repo.CreateUpdateVariety(new Variety {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation,
                    Specie = specie
                }),
                s => s.Name.Equals(name) || s.Abbreviation.Equals(abbreviation),
                $"Ya existe variedad con nombre: {name} o abreviación: {abbreviation}. Ambos campos deben ser unicos."
            );
        }

    }
}
