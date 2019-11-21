using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.fields
{
    public class BarrackOperations : IBarrackOperations
    {
        private readonly string _idSeason;
        private readonly IBarrackRepository _repo;
        private readonly IRootstockRepository _repoRootstock;
        private readonly IPlotLandRepository _repoPlotLand;
        private readonly IVarietyRepository _repoVariety;
        private readonly ICommonDbOperations<Barrack> _commonDb;
        public BarrackOperations(IBarrackRepository repo, IRootstockRepository repoRootstock,IPlotLandRepository repoPlotLand, IVarietyRepository repoVariety, ICommonDbOperations<Barrack> commonDb, string idSeason)
        {
            _repo = repo;
            _repoRootstock = repoRootstock;
            _repoPlotLand = repoPlotLand;
            _repoVariety = repoVariety;
            _commonDb = commonDb;
            _idSeason = idSeason;
        }

        public async Task<ExtGetContainer<Barrack>> GetBarrack(string id)
        {
            try
            {
                var barrack = await _repo.GetBarrack(id);
                
                return OperationHelper.GetElement(barrack);
            }
            catch (Exception e)
            {
                return OperationHelper.GetException<Barrack>(e, e.Message);
            }
        }

        public async Task<ExtGetContainer<List<Barrack>>> GetBarracks()
        {
            try
            {
                var queryBarracks = _repo.GetBarracks();
               
                var barracks = await _commonDb.TolistAsync(queryBarracks);
                return OperationHelper.GetElements(barracks);
            }
            catch (Exception e)
            {
                return OperationHelper.GetException<List<Barrack>>(e, e.Message);
            }
        }

        public async Task<ExtPostContainer<Barrack>> SaveEditBarrack(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock)
        {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator, idRootstock);
            if (!elements.Success) return OperationHelper.PostNotFoundElementException<Barrack>(elements.Message, elements.IdNotfound);
            var element = await _repo.GetBarrack(id);
            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    s.SeasonId = _idSeason;
                    s.PlotLand = elements.PlotLand;
                    s.NumberOfPlants = numberOfPlants;
                    s.PlantingYear = plantingYear;
                    s.Rootstock = elements.Rootstock;
                    s.Pollinator = elements.Pollinator;
                    s.Variety = elements.Variety;
                    s.Hectares = hectares;
                    return s;
                },
                _repo.CreateUpdateBarrack,
                 $"No existe Parcela con id : {id}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewBarrack(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock)
        {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator, idRootstock);

            if (!elements.Success) return OperationHelper.PostNotFoundElementException<string>(elements.Message, elements.IdNotfound);

            return await OperationHelper.CreateElement(_commonDb,_repo.GetBarracks(),
                async s => await _repo.CreateUpdateBarrack(new Barrack
                {
                    Id = s,
                    Name = name,
                    SeasonId = _idSeason,
                    PlotLand = elements.PlotLand,
                    Hectares = hectares,
                    NumberOfPlants = numberOfPlants,
                    PlantingYear = plantingYear,
                    Rootstock = elements.Rootstock,
                    Pollinator = elements.Pollinator,
                    Variety = elements.Variety
                }),
                s => s.Name.Equals(name),
                $"ya existe Cuartel con nombre {name} "

            );

        }

        private async Task<ElementsBarracks> GetElementToBarracks(string idPlotLand, string idVariety, string idVarietyPollinator, string idRootstock) {
            var elementBarrack = new ElementsBarracks();
            elementBarrack.Success = true;
            elementBarrack.PlotLand = await _repoPlotLand.GetPlotLand(idPlotLand);
            if (elementBarrack.PlotLand == null)
            {
                elementBarrack.Message = $"no existe parcela con id {idPlotLand}";
                elementBarrack.IdNotfound = idPlotLand;
                elementBarrack.Success = false;
            }
            elementBarrack.Variety = await _repoVariety.GetVariety(idVariety);
            if (elementBarrack.Variety == null)
            {
                elementBarrack.IdNotfound = idVariety;
                elementBarrack.Message = $"no existe variedad con id {idVariety}";
                elementBarrack.Success = false;
            }
            if (!string.IsNullOrWhiteSpace(idVarietyPollinator))
            {
                elementBarrack.Pollinator = await _repoVariety.GetVariety(idVarietyPollinator);
            }
            if (!string.IsNullOrWhiteSpace(idRootstock))
            {
                elementBarrack.Rootstock = await _repoRootstock.GetRootstock(idRootstock);
            }
            return elementBarrack;
        }

    }

    public class ElementsBarracks {
        public bool Success { get; set; }

        public Variety Variety { get; set; }

        public Variety Pollinator { get; set; }

        public PlotLand PlotLand { get; set; }

        public Rootstock Rootstock { get; set; }

        public string Message { get; set; }

        public string IdNotfound { get; set; }

    }
}
