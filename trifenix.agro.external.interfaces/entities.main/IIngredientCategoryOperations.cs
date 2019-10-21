using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IIngredientCategoryOperations
    {
        Task<ExtPostContainer<string>> SaveNewIngredientCategory(string name);

        Task<ExtPostContainer<IngredientCategory>> SaveEditIngredientCategory(string id, string name);

        Task<ExtGetContainer<List<IngredientCategory>>> GetIngredientCategories();




    }
}
