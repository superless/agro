using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// identifica un año agricola.
    /// </summary>
    [SharedCosmosCollection("agro", "Season")]
    [ReferenceSearchHeader(EntityRelated.SEASON, PathName = "seasons", Kind = EntityKind.CUSTOM_ENTITY)]
    public class Season : DocumentBase, ISharedCosmosEntity {

        /// <summary>
        /// identificador.
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// fecha de inicio
        /// </summary>
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }


        /// <summary>
        /// fecha fin
        /// </summary>
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Identifica si el agricola es el actual.
        /// </summary>
        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

        /// <summary>
        /// identificador del costcenter.
        /// </summary>
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }

}