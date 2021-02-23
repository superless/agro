using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Pre - Orden, una preorden es una orden estimada con menos datos que se asigna al iniciar el dato.
    /// </summary>
    [SharedCosmosCollection("agro", "PreOrder")]
    [ReferenceSearchHeader(EntityRelated.PREORDER, PathName = "pre-orders", Kind = EntityKind.PROCESS)]
    public class PreOrder : DocumentLocal
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Nombre de la pre-orden.
        /// </summary>
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Carpeta a la que pertenece, esto solo aplicará si la pre-orden es de tipo fenológica, 
        /// las que no son fenológica no tienen carpeta.
        /// </summary>
        [ReferenceSearch(EntityRelated.ORDER_FOLDER)]
        public string OrderFolderId { get; set; }

        /// <summary>
        /// Tipo de pre-orden
        /// </summary>
        [EnumSearch(EnumRelated.PRE_ORDER_TYPE)]
        public PreOrderType PreOrderType { get; set; }

        /// <summary>
        /// Barracks asociados a la pre orden
        /// </summary>
        [ReferenceSearch(EntityRelated.BARRACK)]
        public string[] BarrackIds { get; set; }

    }
}


