using Cosmonaut.Extensions;
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

        public async Task<ExtGetContainer<Variety>> GetVariety(string id)
        {
            var order = await _repo.GetVariety(id);
            return OperationHelper.GetElement(order);
        }

        public async Task<ExtPostContainer<Variety>> SaveEditVariety(string id, string name, string abbreviation, string idSpecie)
        {
            var specie = await _repoSpecie.GetSpecie(idSpecie);
            if (specie == null) return OperationHelper.PostNotFoundElementException<Variety>($"no se encontró especie con id {idSpecie}", idSpecie);

            var element = await _repo.GetVariety(id);

            return await OperationHelper.EditElement(_commonDb, _repo.GetVarieties(), 
                id,
                element,
                s => {
                    s.Name = name;
                    s.Abbreviation = abbreviation;
                    s.Specie = specie;
                    return s;
                },
                _repo.CreateUpdateVariety,
                 $"No existe variedad con id : {id}",
                s => s.Name.Equals(name),
                $"Ya existe variedad con nombre {name}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewVariety(string name, string abbreviation, string idSpecie)
        {
            var specie = await _repoSpecie.GetSpecie(idSpecie);
            if (specie == null) return OperationHelper.PostNotFoundElementException<string>($"no se encontró especie con id {idSpecie}", idSpecie);
            return await OperationHelper.CreateElement(_commonDb, _repo.GetVarieties(),
                async s => await _repo.CreateUpdateVariety(new Variety
                {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation,
                    Specie = specie
                }),
                s => s.Name.Equals(name),
                $"Ya existe variedad con nombre {name}"
            );

        }
    }
}
