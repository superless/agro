using Cosmonaut;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents.Spatial;
using System;
using trifenix.agro.db;
using trifenix.agro.weather.model;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{

    /// <summary>
    /// Entidad correspondiente a los mensajes enviados por los dispositivos móviles, en caso de ocurrir un
    /// evento fenológico.
    /// </summary>
    [SharedCosmosCollection("agro", "NotificationEvent")]
    [ReferenceSearchHeader(EntityRelated.NOTIFICATION_EVENT, PathName = "notification_events", Kind = EntityKind.CUSTOM_ENTITY)]
    public class NotificationEvent : DocumentBase<long>, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador de la Notificación
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

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
        [StringSearch(StringRelated.PICTURE_PATH_EVENT)]
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


        //TODO: Revisar si se puede generar el campo en typescript o se debe asignar en otro lado.
        public Weather Weather { get; set; }


        /// <summary>
        /// Ubicación desde donde se generó el evento.
        /// </summary>
        [GeoSearch(GeoRelated.LOCATION_EVENT)]
        public Point Location { get; set; }

    }
}
