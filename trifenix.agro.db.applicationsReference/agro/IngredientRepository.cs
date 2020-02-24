using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class IngredientRepository : MainGenericDb<Ingredient>, IMainGenericDb<Ingredient>
    {
        public IngredientRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
}
