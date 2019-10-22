using trifenix.agro.external.interfaces.entities;
using trifenix.agro.external.interfaces.entities.main;

namespace trifenix.agro.external.interfaces
{
    public interface IAgroManager
    {
        IPhenologicalOperations PhenologicalEvents { get; }

        IApplicationTargetOperations ApplicationTargets { get; }
        
        ISpecieOperations Species { get; }

        IIngredientCategoryOperations IngredientCategories { get; }

        IIngredientsOperations Ingredients { get; }

        ISeasonOperations Seasons { get; }

        IOrderFolderOperations OrderFolder { get; }

        

 

    }
}
