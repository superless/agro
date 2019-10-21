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

        IPhenologicalEventRepositoy PhenologicalEvents { get; }

        ISpecieRepository SpecieRepository { get; }

        ISeasonRepository Seasons { get; }
        




    }
}
