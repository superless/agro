using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.interfaces.Products
{
    public interface IApplicationMethodContainer
    {
        Task<string> CreateUpdateApplicationMethos(ApplicationMethod method);

        Task<ApplicationMethod> GetApplicationMethod(string uniqueId);

        IQueryable<ApplicationMethod> GetApplicationMethods();



    }
}
