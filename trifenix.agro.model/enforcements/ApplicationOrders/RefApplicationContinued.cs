using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.ApplicationOrders
{
    [SharedCosmosCollection("agro", "RefApplicationContinued")]
    public class RefApplicationContinued : ReferenceApplicationOrder, ISharedCosmosEntity
    {
        public string IdPreviousRefApplication { get; set; }

    }
}
