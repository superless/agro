using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// clase local para hacer referencia a variedad.
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.POLLINATOR, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class Pollinator {
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }
    }

}