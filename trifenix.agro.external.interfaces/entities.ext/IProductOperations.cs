
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces.entities.ext
{
    public interface IProductOperations
    {
        Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

    }
}
