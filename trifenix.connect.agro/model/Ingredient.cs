using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargadas de los ingredientes
    /// </summary>
    [SharedCosmosCollection("agro", "Ingredient")]
    [ReferenceSearchHeader(EntityRelated.INGREDIENT, Kind = EntityKind.ENTITY, PathName = "ingredients")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.PRODUCTO)]
    public class Ingredient : DocumentLocal {


        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Nombre del ingrediente.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Categoría del ingrediente.
        /// </summary>
        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string idCategory { get; set; }

    }
}