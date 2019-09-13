using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Helper;
using trifenix.agro.db.model.enforcements.Helper;

namespace trifenix.agro.db.applicationsReference.Helper
{
    public class CounterTask : MainDb<Counter>, ICounterContainer
    {
        public CounterTask(AgroDbArguments args) : base(args)
        {
        }

        public async Task<long> GetNextTaskId()
        {
            var counterFound = await GetEntity("0");

            if (counterFound == null)
            {
                var id = await CreateUpdate(new Counter { Id = "0", NextTaskId = 1 });
                return 1;
            }

            counterFound.NextTaskId = counterFound.NextTaskId + 1;

            await CreateUpdate(counterFound);

            return counterFound.NextTaskId;
        }
    }
}
