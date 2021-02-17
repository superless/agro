using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Identifica un año agricola.
    /// </summary>
    [SharedCosmosCollection("agro", "Season")]
    [ReferenceSearchHeader(EntityRelated.SEASON, PathName = "seasons", Kind = EntityKind.CUSTOM_ENTITY)]
    public class Season : DocumentLocal {

        /// <summary>
        /// Identificador.
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha fin
        /// </summary>
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Identifica si el agricola es el actual.
        /// </summary>
        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

        /// <summary>
        /// Centro de costos que administra la temporada
        /// </summary>
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }

}