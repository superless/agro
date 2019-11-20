using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.custom;
using trifenix.agro.external.operations.custom;

namespace trifenix.agro.external.operations
{
    public class CustomManager : ICustomManager
    {
        private readonly ITimeStampDbQueries tsRepo;
        private readonly ICommonDbOperations<Barrack> dbBarrackOper;
        private readonly ICommonDbOperations<PhenologicalEvent> dbPhenologicalOper;
        private readonly string idSeason;
        private readonly IAgroRepository agroRepository;

        public CustomManager(IAgroRepository agroRepository, string idSeason)
        {
            tsRepo = new TimeStampDbQueries(agroRepository.DbArguments);
            dbBarrackOper = new CommonDbOperations<Barrack>();
            dbPhenologicalOper = new CommonDbOperations<PhenologicalEvent>();
            this.idSeason = idSeason;
            this.agroRepository = agroRepository;

        }
        public IMobileEventCustomElements MobileEvents => new MobileCustomElements(tsRepo, agroRepository.Barracks, agroRepository.PhenologicalEvents, agroRepository.Varieties, dbBarrackOper, dbPhenologicalOper, idSeason);
    }
}
