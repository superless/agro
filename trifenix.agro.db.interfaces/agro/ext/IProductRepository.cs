using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.ext
{
    public interface IProductRepository
    {
        Task<string> CreateUpdateProduct(Product product);

        Task<Product> GetProduct(string id);

        IQueryable<Product> GetProducts();

        Task<long> Total(string season);

    }

}