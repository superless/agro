using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.applicationsReference.Products
{
    public class ApplicationMethodContainer : MainDb<ApplicationMethod>, IApplicationMethodContainer
    {
        public ApplicationMethodContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateApplicationMethos(ApplicationMethod method)
        {
            return await CreateUpdate(method);
        }

        public async Task<ApplicationMethod> GetApplicationMethod(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<ApplicationMethod> GetApplicationMethods()
        {
            return GetEntities();
        }
    }
}
