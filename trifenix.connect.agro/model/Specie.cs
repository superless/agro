using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{

    [SharedCosmosCollection("agro", "Specie")]
    [ReferenceSearchHeader(EntityRelated.SPECIE, PathName = "species", Kind = EntityKind.ENTITY )]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.ESPECIES)]
    public class Specie : DocumentBaseName<long>, ISharedCosmosEntity {
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        /// <summary>
        /// nombre de la especie
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        /// <summary>
        /// abreviación de la especie.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }
    }

}
