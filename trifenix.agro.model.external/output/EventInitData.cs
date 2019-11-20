

using System.Collections.Generic;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.output
{
    public class EventInitData
    {
        public long TimeStamp { get; set; }

        public OutputMobileSector[] Sectors { get; set; }

        private Dictionary<int, OutputMobileEvent[]> _events;

        public Dictionary<int, OutputMobileEvent[]> Events
        {
            get {
                _events = _events ?? new Dictionary<int, OutputMobileEvent[]>();
                return _events; }
            set { _events = value; }
        }


    }

   


    

    public class OutputMobileEvent {

        public string Id { get; set; }

        public string Name { get; set; }
    }


    public class OutputMobileSector {
        public string Id { get; set; }

        public string Name { get; set; }

        public OutputMobileSpecie[] Species { get; set; }


    }

    public class OutputMobileSpecie
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
