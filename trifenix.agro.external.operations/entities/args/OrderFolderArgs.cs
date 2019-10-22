using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.interfaces.agro;

namespace trifenix.agro.external.operations.entities.args
{
    public class OrderFolderArgs
    {

        public IPhenologicalEventRepository PhenologicalEvent { get; set; }

        public IApplicationTargetRepository Target { get; set; }

        public ISpecieRepository Specie { get; set; }

        public IIngredientRepository Ingredient { get; set; }

        public IIngredientCategoryRepository IngredientCategory { get; set; }

        public IOrderFolderRepository OrderFolder { get; set; }

        public ISeasonRepository Season { get; set; }


    }
}
