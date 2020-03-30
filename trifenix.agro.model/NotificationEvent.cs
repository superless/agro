using Cosmonaut;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents.Spatial;
using System;
using trifenix.agro.attr;
using trifenix.agro.db.model.local;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;
using trifenix.agro.weather.model;

namespace trifenix.agro.db.model
{

    /// <summary>
    /// Entidad correspondiente a los mensajes enviados por los dispositivos móviles, en caso de ocurrir un
    /// evento fenológico.
    /// </summary>
    [SharedCosmosCollection("agro", "NotificationEvent")]
    [ReferenceSearch(EntityRelated.NOTIFICATION_EVENT)]
    public class NotificationEvent : DocumentBase, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador de la Notificación
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Cuartel asignado a la notificación
        /// </summary>

        [ReferenceSearch(EntityRelated.BARRACK)]
        public string IdBarrack { get; set; }


        /// <summary>
        /// Evento fenológico asignado a la notificación.
        /// </summary>
        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EnumSearch(EnumRelated.NOTIFICATION_TYPE)]
        public NotificationType NotificationType { get; set; }
        

        /// <summary>
        /// Ruta o Url en internet de la imagen subida.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_PICTURE_PATH)]
        public string PicturePath { get; set; }

        /// <summary>
        /// Descripcion del evento
        /// </summary>
        [StringSearch(StringRelated.GENERIC_DESC)]
        public string Description { get; set; }


        /// <summary>
        /// Fecha de creación.
        /// </summary>
        /// 

        public DateTime Created { get; set; }


        public Weather Weather { get; set; }


        [GeoSearch(GeoRelated.GENERIC_LOCATION)]
        public Point Location { get; set; }

    }
}
