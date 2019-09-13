using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.interfaces.Field
{
    public interface IAgroFieldContainer
    {
        Task<string> CreateUpdateAgroField(AgroField field);

        Task<AgroField> GetAgroField(string uniqueId);


        IQueryable<AgroField> GetAgroFields();

    }
}
