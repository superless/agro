using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class BarrackRepository : MainDb<Barrack>, IBarrackRepository
    {
        public BarrackRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateBarrack(Barrack barrack)
        {
            return await CreateUpdate(barrack);
        }

        public async Task<Barrack> GetBarrack(string id)
        {
           return  await GetEntity(id);
        }

        public IQueryable<Barrack> GetBarracks()
        {
            return GetEntities();
        }
    }
}
