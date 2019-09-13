using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.applicationsReference.Products
{
    public class CertifierRegionContainer : MainDb<CertifierRegion>, ICertifierRegionContainer
    {
        public CertifierRegionContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateCertifierRegion(CertifierRegion region)
        {
            return await CreateUpdate(region);
        }

        public async Task<CertifierRegion> GetCertifierRegion(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<CertifierRegion> GetCertifierRegions()
        {
            return GetEntities();
        }
    }
}
