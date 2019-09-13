using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Field;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.applicationsReference.Field
{
    public class AgroFieldContainer: MainDb<AgroField>, IAgroFieldContainer
    {
        public AgroFieldContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {

        }

        public async Task<string> CreateUpdateAgroField(AgroField field)
        {
            return await CreateUpdate(field);
        }

        public async Task<AgroField> GetAgroField(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<AgroField> GetAgroFields()
        {
            return GetEntities();
        }
    }
}
