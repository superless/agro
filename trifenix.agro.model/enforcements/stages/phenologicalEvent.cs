
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.model.enforcements.stages
{
    public class PhenologicalEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
