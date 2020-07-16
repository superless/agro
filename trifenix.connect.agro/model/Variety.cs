using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.indexes;

namespace trifenix.connect.agro.model
{
    [SharedCosmosCollection("agro", "Variety")]    
    [ReferenceSearchHeader(EntityRelated.VARIETY, Kind = EntityKind.ENTITY, PathName = "varieties")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.ESPECIES)]
    public class Variety : DocumentBaseName<long>, ISharedCosmosEntity
    {
        /// <summary>
        /// identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre de la variedad
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        /// <summary>
        /// abreviación.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }


        /// <summary>
        /// Identificador de una especie.
        /// </summary>
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string IdSpecie { get; set; }

        /// <summary>
        /// autonumérico.
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }
    }
}
