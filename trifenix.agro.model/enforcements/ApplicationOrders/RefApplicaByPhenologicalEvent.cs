using Cosmonaut;
using Cosmonaut.Attributes;
using Newtonsoft.Json;
using trifenix.agro.db.model.enforcements.stages;

namespace trifenix.agro.db.model.enforcements.ApplicationOrders
{
    [SharedCosmosCollection("agro", "RefApplicaByPhenologicalEvent")]
    public class RefApplicaByPhenologicalEvent : ReferenceApplicationOrder, ISharedCosmosEntity
    {
        public PhenologicalEvent PhenologicalEvent { get; set; }
    }
}
