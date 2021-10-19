using System;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// El evento fenológico se debería crear una vez al año o copiarse del año anterior.
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.PHENOLOGICAL_EVENT, Kind = EntityKind.ENTITY, PathName = "phenological_events")]
    
    public class PhenologicalEvent : DocumentDb {

        public override string Id { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Nombre del evento fenológico.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [DateSearch(DateRelated.START_DATE_PHENOLOGICAL_EVENT)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha fin del evento fenológico.
        /// </summary>
        [DateSearch(DateRelated.END_DATE_PHENOLOGICAL_EVENT)]
        public DateTime EndDate { get; set; }
        
    }
}