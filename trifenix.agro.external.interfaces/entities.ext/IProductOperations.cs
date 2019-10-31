
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.ext
{
    public interface IProductOperations
    {
        Task<ExtPostContainer<string>> CreateProduct(string commercialName, string idActiveIngredient, string brand, string[] idDoses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct);

    }
}
