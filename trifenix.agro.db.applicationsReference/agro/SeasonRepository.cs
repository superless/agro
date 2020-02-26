using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using Cosmonaut.Exceptions;
using Cosmonaut.Extensions;
using trifenix.agro.db.interfaces;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SeasonRepository : MainGenericDb<Season>, IMainGenericDb<Season>
    {
        public SeasonRepository(AgroDbArguments args) : base(args)
        {
        }
    }
}
