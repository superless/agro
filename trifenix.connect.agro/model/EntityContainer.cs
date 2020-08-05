using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{


    // TODO : Cristian, documentar.
    [SharedCosmosCollection("agro", "EntityContainer")]
    public class EntityContainer : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        public dynamic Entity { get; set; }

    }

}