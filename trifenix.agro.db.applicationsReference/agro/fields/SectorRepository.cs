using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class SectorRepository : MainGenericDb<Sector>, IMainGenericDb<Sector>
    {
        public SectorRepository(AgroDbArguments dbArguments):base(dbArguments)
        {
            
        }
    }
}
