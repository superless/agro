using System.Collections.Generic;
using trifenix.agro.db.model.enforcements.Fields;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.model.enforcements.Applications
{
    public class ApplicationInField
    {

        public AgroField Field { get; set; }


        /// <summary>
        /// Wetting by hectare
        /// </summary>
        public long WettingByHec { get; set; }

        private List<ActiveIngredientInstance> _ingredientInstance;

        public List<ActiveIngredientInstance> IngredientInstances
        {
            get {
                _ingredientInstance = _ingredientInstance ?? new List<ActiveIngredientInstance>();
                return _ingredientInstance; }
            set { _ingredientInstance = value; }
        }


        private List<ActiveIngredientCategoryInstance> _categoryInstance;

        public List<ActiveIngredientCategoryInstance> CategoryInstance
        {
            get {
                _categoryInstance = _categoryInstance ?? new List<ActiveIngredientCategoryInstance>();
                return _categoryInstance; }
            set { _categoryInstance = value; }
        }




    }
}
