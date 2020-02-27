using System.Collections.Generic;
using System.Threading.Tasks;

using trifenix.agro.enums;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;

namespace trifenix.agro.external.interfaces.entities.ext {
    public interface IProductOperations <T> {

        Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

        Task<ExtPostContainer<T>> SaveEditProduct(string id, string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

        Task<ExtGetContainer<List<T>>> GetProducts();

        Task<ExtGetContainer<T>> GetProduct(string id);

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, int? page, int? quantity, bool? desc);

        ExtGetContainer<SearchResult<T>> GetPaginatedProducts(string textToSearch, int? page, int? quantity, bool? desc);

    }
}