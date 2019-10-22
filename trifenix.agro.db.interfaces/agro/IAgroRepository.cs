using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IAgroRepository
    {
        
        IApplicationTargetRepository Targets { get; }

        IIngredientCategoryRepository Categories { get; }

        IIngredientRepository Ingredients { get; }

        IPhenologicalEventRepository PhenologicalEvents { get; }

        ISpecieRepository Species{ get; }

        ISeasonRepository Seasons { get; }

        IOrderFolderRepository OrderFolder { get; }
        




    }
}
