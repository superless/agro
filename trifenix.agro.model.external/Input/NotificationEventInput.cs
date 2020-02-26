using System;
using trifenix.agro.enums;
using trifenix.agro.weather.model;

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
        /// Ruta o Url en internet de la imagen subida.
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// Descripcion del evento
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime Created { get; set; }


        public float? Lat { get; set; }

        public float? Long { get; set; }


    }

    
}
