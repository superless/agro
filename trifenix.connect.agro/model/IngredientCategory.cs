using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de las categorias de los ingredientes
    /// </summary>
    [SharedCosmosCollection("agro", "IngredientCategory")]
    [ReferenceSearchHeader(EntityRelated.CATEGORY_INGREDIENT, PathName = "ingredient_categories", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.PRODUCTO)]
    public class IngredientCategory : DocumentLocal
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


    }
}
