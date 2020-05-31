using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input
{
    [ReferenceSearchHeader(EntityRelated.NOTIFICATION_EVENT)]
    public class NotificationEventInput : InputBase {
        /// <summary>
        /// Cuartel asignado a la notificación
        /// </summary>
        /// 
        [ReferenceSearch(EntityRelated.BARRACK)]
        public string IdBarrack { get; set; }
        /// <summary>
        /// Evento fenológico asignado a la notificación.
        /// </summary>
        /// [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }


        [EnumSearch(EnumRelated.NOTIFICATION_TYPE)]
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// Descripcion del evento
        /// </summary>
        /// 
        [StringSearch(StringRelated.GENERIC_DESC)]
        public string Description { get; set; }
        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime Created { get; set; }

        public string Base64 { get; set; }

        [GeoSearch(GeoRelated.LOCATION_EVENT)]
        public GeographicalPointInput Location { get; set; }

    }

    

}