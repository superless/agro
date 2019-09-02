using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.model.enforcements.stages;

namespace trifenix.agro.model.enforcements.ApplicationOrders
{
    public class ReferenceApplicationOrder
    {
        public string ApplicationName { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }

        public ApplicationPurpose applicationPurpose { get; set; }



    }
}
