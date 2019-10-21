using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Field;
using trifenix.agro.db.model.enforcements;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.applicationsReference.Field
{
    public class AgroSpecieContainer : MainDb<AgroSpecie>, IAgroSpecieContainer
    {
        public AgroSpecieContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateSpecie(AgroSpecie specie)
        {
            return await CreateUpdate(specie);
        }

        public async Task<AgroSpecie> Getspecie(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<AgroSpecie> GetSpecies()
        {
            return GetEntities();
        }
    }
}
