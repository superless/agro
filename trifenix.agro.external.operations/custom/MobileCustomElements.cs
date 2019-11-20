using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces.custom;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;

namespace trifenix.agro.external.operations.custom
{
    public class MobileCustomElements : IMobileEventCustomElements
    {
        private readonly ITimeStampDbQueries ts;
        private readonly IBarrackRepository barrackRepository;
        private readonly IPhenologicalEventRepository phenologicalRepository;
        private readonly IVarietyRepository varietyRepository;
        private readonly ICommonDbOperations<Barrack> barrackOper;
        private readonly ICommonDbOperations<PhenologicalEvent> phenologicalOper;
        private readonly string idSeason;

        public MobileCustomElements(ITimeStampDbQueries ts, IBarrackRepository barrackRepository, IPhenologicalEventRepository phenologicalRepository, IVarietyRepository varietyRepository, ICommonDbOperations<Barrack> barrackOper, ICommonDbOperations<PhenologicalEvent> phenologicalOper, string idSeason)
        {
            this.ts = ts;
            this.barrackRepository = barrackRepository;
            this.phenologicalRepository = phenologicalRepository;
            this.varietyRepository = varietyRepository;
            this.barrackOper = barrackOper;
            this.phenologicalOper = phenologicalOper;
            this.idSeason = idSeason;
        }
        public async  Task<ExtGetContainer<EventInitData>> GetEventData()
        {
            var barracksQuery = barrackRepository.GetBarracks().Where(s=>s.SeasonId == idSeason);
            var barracks = await barrackOper.TolistAsync(barracksQuery);
            var phenologicalQuery = phenologicalRepository.GetPhenologicalEvents();
            var phenologicals = await phenologicalOper.TolistAsync(phenologicalQuery);

            var init =  await GetEventInitData(barracks, phenologicals);
            return OperationHelper.GetElement(init);
        }

        private async Task<EventInitData> GetEventInitData(List<Barrack> barracks, List<PhenologicalEvent> phenologicalEvents) {
            
            var tsResult = await GetMobileEventTimestamp();

            var sectors = GetSectors(barracks);


            return new EventInitData
            {
                TimeStamp = tsResult.Result,
                Sectors = sectors,
                Events = new Dictionary<int, OutputMobileEvent[]> {
                    { (int)KindEvent.PhenologicalEvent ,phenologicalEvents.Select(s => new OutputMobileEvent
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToArray()}
                }
                
            };

             
        }


        
        private OutputMobileSector[] GetSectors(List<Barrack> barracks) {


             return barracks.GroupBy(s => s.PlotLand.Sector.Id).Select( s=> {

                 var species = GetVarieties(s.ToList()).ToArray();


                 return new OutputMobileSector
                 {
                     Id = s.Key,
                     Name = s.First().PlotLand.Sector.Name,
                     Species = species
                 };


             }).ToArray();


        }

        private async Task<OutputMobileSpecie> GetSpecie(string idVariety)
        {
            var variety = await varietyRepository.GetVariety(idVariety);

            return new OutputMobileSpecie
            {
                Id = variety.Specie.Id,
                Name = variety.Specie.Name
            };

        }



       

        private IEnumerable<OutputMobileSpecie> GetVarieties(List<Barrack> barracks) {




            var specieList = barracks.GroupBy(s => s.Variety.Id).Select(async s =>
            {

                var specie = await GetSpecie(s.Key);


                return new OutputMobileSpecie
                {
                    Id = specie.Id,
                    Name = specie.Name,
                    Barracks = GetBarracks(s.ToList())
                };
            });

            return specieList.GroupBy(s => s.Result.Id).Select(v => new OutputMobileSpecie
            {
                Id = v.Key,
                Name = v.First().Result.Name,
                Barracks = v.SelectMany(a => a.Result.Barracks).GroupBy(b => b.Id).Select(f => f.First()).ToArray()
            });


        }

        private OutputMobileBarrack[] GetBarracks(List<Barrack> barracks)
        {
            return barracks.GroupBy(s => s.Id).Select(s => new OutputMobileBarrack {
                Id = s.Key,
                Name = s.First().Name
            }).ToArray();
        }



        public async Task<ExtGetContainer<long>> GetMobileEventTimestamp()
        {
            var tsBarracks = await ts.GetTimestamps<Barrack>();
            var tsPhenological = await ts.GetTimestamps<PhenologicalEvent>();

            return OperationHelper.GetElement(tsBarracks.Union(tsPhenological).Max());



        }
    }
}
