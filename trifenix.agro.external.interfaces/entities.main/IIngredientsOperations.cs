using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IIngredientsOperations
    {
        Task<ExtPostContainer<string>> SaveNewIngredient(string name, string categoryId);

        Task<ExtPostContainer<Ingredient>> SaveEditIngredient(string id, string name, string categoryId);

        Task<ExtGetContainer<List<Ingredient>>> GetIngredients();

        
    }
}