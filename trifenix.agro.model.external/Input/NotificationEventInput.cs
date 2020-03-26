using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class NotificationEventInput : InputBase {
        /// <summary>
        /// Cuartel asignado a la notificación
        /// </summary>
        public string IdBarrack { get; set; }
        /// <summary>
        /// Evento fenológico asignado a la notificación.
        /// </summary>
        public string IdPhenologicalEvent { get; set; }

        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// Descripcion del evento
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime Created { get; set; }

        public string Base64 { get; set; }

        public float? Lat { get; set; }

        public float? Long { get; set; }

    }

    public class NotificationEventSwaggerInput {
        
        [Required]
        public string IdBarrack { get; set; }
        
        public string IdPhenologicalEvent { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public string Base64 { get; set; }

        public float? Lat { get; set; }

        public float? Long { get; set; }

    }

}