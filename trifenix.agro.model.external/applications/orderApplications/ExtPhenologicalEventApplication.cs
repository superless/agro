using System;
using trifenix.agro.db.model.enforcements.stages;

namespace trifenix.agro.model.external.applications
{
    public class ExtPhenologicalEventApplication : ExtReferenceApplication
    {
        public PhenologicalEvent PhenologicalEvent { get; set; }
        public DateTime StartDate { get; set; }

    }

}
