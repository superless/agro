using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.entities.events
{
    public class NotificationEventOperations : INotificatonEventOperations
    {
        private readonly IPhenologicalEventRepository _phenologicalRepository;
        private readonly IBarrackRepository _barrackRepository;
        private readonly IUploadImage _uploadImage;
        private readonly INotificationEventRepository _repo;

        public NotificationEventOperations(INotificationEventRepository repo, IBarrackRepository barrackRepository, IPhenologicalEventRepository phenologicalRepository, IUploadImage uploadImage = null)
        {
            _repo = repo;
            _phenologicalRepository = phenologicalRepository;
            _barrackRepository = barrackRepository;
            _uploadImage = uploadImage;
        }


        public async Task<ExtGetContainer<NotificationEvent>> GetEvent(string id)
        {
            var notEvent = await _repo.GetNotificationEvent(id);
            return OperationHelper.GetElement(notEvent);
        }

        public async Task<ExtGetContainer<List<NotificationEvent>>> GetEvents()
        {
            var notEvents = await _repo.GetNotificationEvents().ToListAsync();
            return OperationHelper.GetElements(notEvents);
        }

        public async Task<ExtPostContainer<string>> SaveNewNotificationEvent(string idBarrack, string idPhenologicalEvent, string base64, string description)
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

            return await OperationHelper.CreateElement(_repo.GetNotificationEvents(),
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
    }
}
