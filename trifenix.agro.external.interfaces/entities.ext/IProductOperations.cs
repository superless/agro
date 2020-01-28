
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.ext
{
    public interface IProductOperations
    {
        Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

        Task<ExtPostContainer<Product>> CreateEditProduct(string id, string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

        Task<ExtGetContainer<List<Product>>> GetProducts();

        Task<ExtGetContainer<Product>> GetProduct(string id);

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, int page, int quantity, bool desc);

        Task<ExtGetContainer<SearchResult<Product>>> GetProductsByPage(int page, int quantity, bool desc);

        Task<ExtGetContainer<SearchResult<Product>>> GetProductsByPage(string textToSearch, int page, int quantity, bool desc);

    }
}
