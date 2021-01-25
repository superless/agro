using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using System;

namespace trifenix.connect.agro_model
{

    [SharedCosmosCollection("agro", "Test")]
    [ReferenceSearchHeader(EntityRelated.TEST, PathName = "test", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.ESPECIES)]
    public class Test : DocumentLocal
    {
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// nombre de la especie
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        /// <summary>
        /// abreviación de la especie.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }
  

        [ReferenceSearch(EntityRelated.BRAND)]
        public string IdBrand { get; set; }


        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }


        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        [BoolSearch(BoolRelated.CURRENT)]
        public bool? Activo { get;  set; }
    }

}
