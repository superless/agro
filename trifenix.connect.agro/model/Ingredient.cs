using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    [SharedCosmosCollection("agro", "Ingredient")]
    [ReferenceSearchHeader(EntityRelated.INGREDIENT, Kind = EntityKind.ENTITY, PathName = "ingredients")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.PRODUCTO)]
    public class Ingredient : DocumentBaseName<long>, ISharedCosmosEntity {


        /// <summary>
        /// id
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// autonumérico
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        /// <summary>
        /// nombre del ingrediente.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        /// <summary>
        /// categoría del ingrediente.
        /// </summary>
        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string idCategory { get; set; }

    }
}