using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Carpeta de órdenes
    /// </summary>
    [SharedCosmosCollection("agro", "OrderFolder")]
    [ReferenceSearchHeader(EntityRelated.ORDER_FOLDER, PathName = "order_folders", Kind = EntityKind.ENTITY)]
    public class OrderFolder : DocumentLocal
    {

        /// <summary>
        /// Identificador de la carpeta
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual autonumérico
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Evento Fenológico
        /// </summary>
        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }

        /// <summary>
        /// Id del objetivo de aplicación.
        /// </summary>
        [ReferenceSearch(EntityRelated.TARGET)]
        public string IdApplicationTarget { get; set; }


        /// <summary>
        /// Identificador de la especie
        /// </summary>
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string IdSpecie { get; set; }

        /// <summary>
        /// Identificador del ingrediente a asignar
        /// </summary>
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string  IdIngredient { get; set; }

    }
}