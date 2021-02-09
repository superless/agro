using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Días de espera antes de cosechar
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.WAITINGHARVEST, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class WaitingHarvest {

        /// <summary>
        /// Partes por millón, esto se incorporó después.
        /// </summary>
        [DoubleSearch(DoubleRelated.PPM)]
        public double Ppm { get; set; }

        /// <summary>
        /// Días de espera antes de la cosecha
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int WaitingDays { get; set; }


        /// <summary>
        /// Entidad certificadora (opcional), 
        /// si es indicado en la etiqueta, 
        /// probablemente no sea de una entidad certificadora.
        /// </summary>
        [ReferenceSearch(EntityRelated.CERTIFIED_ENTITY)]
        public string IdCertifiedEntity { get; set; }

        /// <summary>
        /// Id de las dosis
        /// </summary>
        [ReferenceSearch(EntityRelated.DOSES)]
        public string IdDoses { get; set; }


    }

}