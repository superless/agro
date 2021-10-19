using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de espera de cosecha
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.WAITINGHARVEST, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class WaitingHarvestInput {

        /// <summary>
        /// Entidad certificadora (opcional), si es indicado en la etiqueta, probablemente no sea de una entidad certificadora.
        /// </summary>
        [ReferenceSearch(EntityRelated.CERTIFIED_ENTITY)]
        [Reference(typeof(CertifiedEntity))]
        public string IdCertifiedEntity { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        [DoubleSearch(DoubleRelated.PPM)]        
        public double Ppm { get; set; }

        /// <summary>
        /// días de espera antes de la cosecha
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int WaitingDays { get; set; }

    }
}