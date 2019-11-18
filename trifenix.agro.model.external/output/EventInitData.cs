

namespace trifenix.agro.model.external.output
{
    public class EventInitData
    {
        public long TimeStamp { get; set; }

        public OutputMobileSector[] Sectors { get; set; }

        public OutputMobilePhenologicalEvent[] PhenologicalEvents { get; set; }

    }


    public class OutputMobilePhenologicalEvent {

        public string Id { get; set; }

        public string Name { get; set; }
    }


    public class OutputMobileSector {
        public string Id { get; set; }

        public string Name { get; set; }

        public OutputMobileVariety[] Varieties { get; set; }


    }

    public class OutputMobileVariety
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public OutputMobileBarrack[] Barracks { get; set; }
    }

    public class OutputMobileBarrack {

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
