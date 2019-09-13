using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.interfaces.Products
{
    public interface ICertifierRegionContainer
    {
        Task<string> CreateUpdateCertifierRegion(CertifierRegion region);


        Task<CertifierRegion> GetCertifierRegion(string uniqueId);

        IQueryable<CertifierRegion> GetCertifierRegions();
    }
}
