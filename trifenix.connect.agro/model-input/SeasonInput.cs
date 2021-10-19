using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de temporadas
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.SEASON)]
    public class SeasonInput : LocalBase
    {

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [Required]
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de término
        /// </summary>
        [Required]
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Función que se encarga de determinar si la temporada está activa o no
        /// </summary>
        [Required]
        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

        /// <summary>
        /// Centro de costos que administra la temporada
        /// </summary>
        [Required, Reference(typeof(CostCenter))]
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }
    }
}