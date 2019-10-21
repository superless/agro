using System;
using System.Collections.Generic;
using trifenix.agro.model.external.applications;

namespace trifenix.agro.model.external.applications
{
    public class ExtPhenologicalEvent
    {

        public string Suffix => "EV";

        public string UniqueId { get; set; }

        public string TaskID { get; set; }

        public string TaskName { get; set; }

        public DateTime StartDate { get; set; }



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
