using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.fields {
    public interface IBarrackOperations <T> {

        Task<ExtGetContainer<T>> GetBarrack(string id);

        Task<ExtGetContainer<List<T>>> GetBarracks();

        Task<ExtPostContainer<string>> SaveNewBarrack(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock);

        Task<ExtPostContainer<T>> SaveEditBarrack(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock);

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, int? page, int? quantity, bool? desc);

        ExtGetContainer<SearchResult<T>> GetPaginatedBarracks(string textToSearch, string abbSpecie, int? page, int? quantity, bool? desc);

    }
}