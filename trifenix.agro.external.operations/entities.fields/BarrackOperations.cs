using System;
using trifenix.agro.model.external.output;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.search.model;
using System.Collections.Generic;
using trifenix.agro.search.operations;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations.entities.fields {
    public class BarrackOperations <T> : IBarrackOperations <T> where T : Barrack {
        private readonly string _idSeason;
        private readonly IBarrackRepository _repo;
        private readonly IRootstockRepository _repoRootstock;
        private readonly IPlotLandRepository _repoPlotLand;
        private readonly IVarietyRepository _repoVariety;
        private readonly ICommonDbOperations<T> _commonDb;
        private readonly IAgroSearch _searchServiceInstance;
        private readonly string entityName = typeof(T).Name;
        public BarrackOperations(IBarrackRepository repo, IRootstockRepository repoRootstock,IPlotLandRepository repoPlotLand, IVarietyRepository repoVariety, ICommonDbOperations<T> commonDb, string idSeason, IAgroSearch searchServiceInstance)
        {
            _repo = repo;
            _repoRootstock = repoRootstock;
            _repoPlotLand = repoPlotLand;
            _repoVariety = repoVariety;
            _commonDb = commonDb;
            _idSeason = idSeason;
            _searchServiceInstance = searchServiceInstance;
        }

        public async Task<ExtGetContainer<T>> GetBarrack(string id) {
            try {
                var barrack = (T)await _repo.GetBarrack(id);
                return OperationHelper.GetElement(barrack);
            }
            catch (Exception e) {
                return OperationHelper.GetException<T>(e);
            }
        }

        public async Task<ExtGetContainer<List<T>>> GetBarracks() {
            var barracksQuery = (IQueryable<T>)_repo.GetBarracks();
            var barracks = await _commonDb.TolistAsync(barracksQuery);
            return OperationHelper.GetElements(barracks);
        }

        public async Task<ExtPostContainer<string>> SaveNewBarrack(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator, idRootstock);
            if (!elements.Success) return OperationHelper.PostNotFoundElementException<string>(elements.Message, elements.IdNotfound);
            var createOperation = await OperationHelper.CreateElement(_commonDb, (IQueryable<T>)_repo.GetBarracks(),
                async s => await _repo.CreateUpdateBarrack(new Barrack {
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
            if (createOperation.GetType() == typeof(ExtPostErrorContainer<string>))
                return OperationHelper.GetPostException<string>(new Exception(createOperation.Message));
            _searchServiceInstance.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = createOperation.IdRelated,
                    SeasonId = _idSeason,
                    Created = DateTime.Now,
                    EntityIndex = entityName,
                    Name = name,
                    Specie = elements.Variety.Specie.Abbreviation
                }
            });
            return createOperation;
        }

        public async Task<ExtPostContainer<T>> SaveEditBarrack(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var elements = await GetElementToBarracks(idPlotLand, idVariety, idPollinator, idRootstock);
            if (!elements.Success) return OperationHelper.PostNotFoundElementException<T>(elements.Message, elements.IdNotfound);
            T barrack = (T)await _repo.GetBarrack(id);
            return await OperationHelper.EditElement<T>(_commonDb, (IQueryable<T>)_repo.GetBarracks(), id,
                barrack,
                s => {
                    _searchServiceInstance.AddElements(new List<EntitySearch> {
                        new EntitySearch{
                            Id = s.Id,
                            Name = name,
                            Specie = elements.Variety.Specie.Abbreviation
                        }
                    });
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
                 $"No existe cuartel con id : {id}",
                s => s.Name.Equals(name) && name!=barrack.Name,
                $"Ya existe un cuartel con nombre {name}"
            );

        }

        public ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = entityName, SeasonId = _idSeason };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetPaginatedEntities(parameters);
            return OperationHelper.GetElement(entitySearch);
        }

        public ExtGetContainer<SearchResult<T>> GetPaginatedBarracks(string textToSearch, string abbSpecie, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = entityName, SeasonId = _idSeason };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetPaginatedEntities(parameters);
            var resultDb = entitySearch.Entities.Select(async s => await GetBarrack(s.Id));
            return OperationHelper.GetElement(new SearchResult<T> {
                Total = entitySearch.Total,
                Elements = resultDb.Select(s => s.Result.Result).ToArray()
            });
        }

        private async Task<ElementsBarracks> GetElementToBarracks(string idPlotLand, string idVariety, string idVarietyPollinator, string idRootstock) {
            var elementBarrack = new ElementsBarracks();
            elementBarrack.Success = true;
            elementBarrack.PlotLand = await _repoPlotLand.GetPlotLand(idPlotLand);
            if (elementBarrack.PlotLand == null) {
                elementBarrack.Message = $"no existe parcela con id {idPlotLand}";
                elementBarrack.IdNotfound = idPlotLand;
                elementBarrack.Success = false;
            }
            elementBarrack.Variety = await _repoVariety.GetVariety(idVariety);
            if (elementBarrack.Variety == null) {
                elementBarrack.IdNotfound = idVariety;
                elementBarrack.Message = $"no existe variedad con id {idVariety}";
                elementBarrack.Success = false;
            }
            if (!string.IsNullOrWhiteSpace(idVarietyPollinator))
                elementBarrack.Pollinator = await _repoVariety.GetVariety(idVarietyPollinator);
            if (!string.IsNullOrWhiteSpace(idRootstock))
                elementBarrack.Rootstock = await _repoRootstock.GetRootstock(idRootstock);
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
