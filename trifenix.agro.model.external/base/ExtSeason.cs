using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.model.external.applications;

namespace trifenix.agro.model.external.@base
{
    public class ExtSeason
    {
        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        private List<ExtPhenologicalEvent> _phenologicalEvents;

        public List<ExtPhenologicalEvent> PhenologicalEvents
        {
            get {
                _phenologicalEvents = _phenologicalEvents ?? new List<ExtPhenologicalEvent>();
                return _phenologicalEvents; }
            set { _phenologicalEvents = value; }
        }

        public DateOrder DateOrder { get; set; }


    }

    public class DateOrder {
        

        public string UniqueId { get; set; }

        public string TaskID => "OPF";

        public string TaskName { get; set; }


        private List<ExtApplicationPurpose> _subTaks;

        public List<ExtApplicationPurpose> SubTasks
        {
            get
            {
                _subTaks = _subTaks ?? new List<ExtApplicationPurpose>();
                return _subTaks;
            }
            set { _subTaks = value; }
        }

    }
}
