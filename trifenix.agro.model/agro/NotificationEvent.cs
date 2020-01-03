using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.microsoftgraph.model;
using trifenix.agro.weather.model;

namespace trifenix.agro.db.model.agro
{

    /// <summary>
    /// Entidad correspondiente a los mensajes enviados por los dispositivos móviles, en caso de ocurrir un
    /// evento fenológico.
    /// </summary>
    [SharedCosmosCollection("agro", "NotificationEvent")]
    public class NotificationEvent : DocumentBase, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador de la Notificación
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Cuartel asignado a la notificación
        /// </summary>
        public Barrack Barrack { get; set; }


        /// <summary>
        /// Evento fenológico asignado a la notificación.
        /// </summary>
        public PhenologicalEvent PhenologicalEvent { get; set; }

        public UserActivity Creator { get; set; }

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

        public Weather Weather { get; set; }

    }
}
