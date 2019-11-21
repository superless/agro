using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.entities.events
{

    /// <summary>
    /// Todos las funciones necesarias para interactuar con eventos registrados en el monitoreo.
    /// </summary>
    public class NotificationEventOperations : INotificatonEventOperations
    {
        private readonly IPhenologicalEventRepository _phenologicalRepository;
        private readonly ICommonDbOperations<NotificationEvent> _commonDb;
        private readonly IBarrackRepository _barrackRepository;
        private readonly IUploadImage _uploadImage;
        private readonly INotificationEventRepository _repo;


        /// <summary>
        /// Constructor de la gestión de notificaciones
        /// </summary>
        /// <param name="repo">Repositorio de base de datos de las notificaciones</param>
        /// <param name="barrackRepository">repositorio de cuarteles</param>
        /// <param name="phenologicalRepository">repositorio de eventos fenológicos</param>
        /// <param name="phenologicalRepository">repositorio de eventos fenológicos</param>
        /// <param name="uploadImage">Objeto que permite obtener la imagen subida en la aplicación</param>
        public NotificationEventOperations(INotificationEventRepository repo, IBarrackRepository barrackRepository, IPhenologicalEventRepository phenologicalRepository, ICommonDbOperations<NotificationEvent> commonDb, IUploadImage uploadImage = null)
        {
            _repo = repo;
            _phenologicalRepository = phenologicalRepository;
            _commonDb = commonDb;
            _barrackRepository = barrackRepository;
            _uploadImage = uploadImage;
        }


        /// <summary>
        /// Obtiene el elemento de notificación de acuerdo al identificador
        /// </summary>
        /// <param name="id">identificador del elemento</param>
        /// <returns>Contenedor con el id del elemento o el detalle del error.</returns>
        public async Task<ExtGetContainer<NotificationEvent>> GetEvent(string id)
        {
            try
            {
                var notEvent = await _repo.GetNotificationEvent(id);
                return OperationHelper.GetElement(notEvent);
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<NotificationEvent>(e, e.Message);
            }
        }


        /// <summary>
        /// obtiene todas las notificaciones de evento como lista en un contenedor
        /// </summary>
        /// <returns>contenedor con la lista de eventos</returns>
        public async Task<ExtGetContainer<List<NotificationEvent>>> GetEvents()
        {

            try
            {
                return await GetEventsWrapper(_repo.GetNotificationEvents());
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<List<NotificationEvent>>(e, e.Message);
            }
        }

        private async Task<ExtGetContainer<List<NotificationEvent>>> GetEventsWrapper(IQueryable<NotificationEvent> notificationQuery) {
            if (notificationQuery == null)
            {
                var message = "La base de datos retorna nulo para eventos";
                return OperationHelper.GetException<List<NotificationEvent>>(new Exception(message), message);
            }

            var notEvents = await _commonDb.TolistAsync(notificationQuery);
            return OperationHelper.GetElements(notEvents);
        }

        public async  Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackId(string id)
        {
            
            try
            {
                return await GetEventsWrapper(_repo.GetNotificationEvents().Where(s => s.Barrack.Id.Equals(id)));
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<List<NotificationEvent>>(e, e.Message);
            }
        }

        public async Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackPhenologicalEventId(string idBarrack, string idPhenologicalId)
        {
            try
            {
                return await GetEventsWrapper(_repo.GetNotificationEvents().Where(s => s.Barrack.Id.Equals(idBarrack) && s.PhenologicalEvent.Id.Equals(idPhenologicalId)));
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<List<NotificationEvent>>(e, e.Message);
            }
        }


        /// <summary>
        /// Almacena en la base de datos una nueva notificación
        /// </summary>
        /// <param name="idBarrack">identificador del cuartel</param>
        /// <param name="idPhenologicalEvent">identificador del evento fenológico</param>
        /// <param name="base64">string de la imagen subida</param>
        /// <param name="description">descripción del evento notificado</param>
        /// <returns>contenedor con el identificador de la notificación</returns>
        public async Task<ExtPostContainer<string>> SaveNewNotificationEvent(string idBarrack, string idPhenologicalEvent, string base64, string description)
        {
            if (string.IsNullOrWhiteSpace(idBarrack) || string.IsNullOrWhiteSpace(idPhenologicalEvent) || string.IsNullOrWhiteSpace(base64))
            {
                return OperationHelper.PostNotFoundElementException<string>($"identificador de barrack o de evento fenologico o la imagen son nulos", idPhenologicalEvent);
            }
            try
            {
                var localBarrack = await _barrackRepository.GetBarrack(idBarrack);
                if (localBarrack == null) return OperationHelper.PostNotFoundElementException<string>($"no se encontró cuartel con id {idBarrack}", idBarrack);

                var localPhenological = await _phenologicalRepository.GetPhenologicalEvent(idPhenologicalEvent);
                if (localPhenological == null) return OperationHelper.PostNotFoundElementException<string>($"no se encontró evento fenológico con id {idPhenologicalEvent}", idPhenologicalEvent);

                string imgPath = string.Empty;
                if (_uploadImage != null)
                {
                    imgPath = await _uploadImage.UploadImageBase64(base64);
                }

                return await OperationHelper.CreateElement(_commonDb, _repo.GetNotificationEvents(),
                   async s => await _repo.CreateUpdateNotificationEvent(new NotificationEvent
                   {
                       Id = s,
                       Barrack = localBarrack,
                       Created = DateTime.Now,
                       Description = description,
                       PhenologicalEvent = localPhenological,
                       PicturePath = imgPath
                   }),
                   s => false,
                   $""
               );
            }
            catch (Exception e)
            {
                return OperationHelper.GetPostException<string>(e);
            }

        }
        
    }
}
