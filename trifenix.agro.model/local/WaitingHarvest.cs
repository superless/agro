using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.local {
    /// <summary>
    /// Días de espera antes de cosechar
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.WAITINGHARVEST, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class WaitingHarvest {

        [DoubleSearch(DoubleRelated.PPM)]
        public double Ppm { get; set; }

        /// <summary>
        /// días de espera antes de la cosecha
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int WaitingDays { get; set; }


        /// <summary>
        /// Entidad certificadora (opcional), si es indicado en la etiqueta, probablemente no sea de una entidad certificadora.
        /// </summary>
        [ReferenceSearch(EntityRelated.CERTIFIED_ENTITY)]
        public string IdCertifiedEntity { get; set; }

    }

}