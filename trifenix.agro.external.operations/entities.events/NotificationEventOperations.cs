using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.email.interfaces;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.agro.weather.interfaces;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.events
{

    /// <summary>
    /// Todos las funciones necesarias para interactuar con eventos registrados en el monitoreo.
    /// </summary>
    public class NotificationEventOperations : MainOperation<NotificationEvent, NotificationEventInput>, IGenericOperation<NotificationEvent, NotificationEventInput> {
        
        private readonly ICommonQueries commonQueries;
        private readonly IEmail email;
        private readonly IUploadImage uploadImage;
        private readonly IWeatherApi weather;

        public NotificationEventOperations(IMainGenericDb<NotificationEvent> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonQueries commonQueries, IEmail email, IUploadImage uploadImage, IWeatherApi weather, ICommonDbOperations<NotificationEvent> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.commonQueries = commonQueries;
            this.email = email;
            this.uploadImage = uploadImage;
            this.weather = weather;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        //public async Task<string> Validate(NotificationEventInput input) {
        //    string errors = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(input.Id)) {  //PUT
        //        var existsId = await existElement.ExistsById<NotificationEvent>(input.Id, false);
        //        if (!existsId)
        //            errors += $"No existe notificación con id {input.Id}.";
        //    }
        //    var existsBarrack = await existElement.ExistsById<Barrack>(input.IdBarrack, false);
        //    if (!existsBarrack)
        //        errors += $"No existe cuartel con id {input.IdBarrack}.";
        //    if (input.NotificationType == NotificationType.Phenological) {
        //        var existsPhenologicalEvent = await existElement.ExistsById<PhenologicalEvent>(input.IdPhenologicalEvent, false);
        //        if (!existsPhenologicalEvent)
        //            errors += $"No existe evento fenológico con id {input.IdPhenologicalEvent}.";
        //    }
        //    return errors.Replace(".",".\r\n");  
        //}

        public async Task<ExtPostContainer<string>> Save(NotificationEvent notificationEvent) {
            //TODO: Revisar
            var picturePath = await uploadImage.UploadImageBase64(notificationEvent.PicturePath);
            notificationEvent.PicturePath = picturePath;
            await repo.CreateUpdate(notificationEvent);
            search.AddDocument(notificationEvent);

            //TODO: Definir el origen de la lista de idsRoles
            var usersEmails = await commonQueries.GetUsersMailsFromRoles(new List<string> { "24beac75d4bb4f8d8fae8373426af780" });
            email.SendEmail(usersEmails, "Notificacion",
                $@"<html>
                    <body>
                        <p> Estimado(a), </p>
                        <p> Llego una notificacion </p>
                        <img src='{picturePath}' style='width:50%;height:auto;'>
                        <p> Atentamente,<br> -Aresa </br></p>
                    </body>
                </html>");
            return new ExtPostContainer<string> {
                IdRelated = notificationEvent.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(NotificationEventInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            NotificationEvent notification = new NotificationEvent {
                Id = id,
                Created = DateTime.Now,
                IdBarrack = input.IdBarrack,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                NotificationType = input.NotificationType,
                PicturePath = input.Base64,
                Description = input.Description,
            };
            if (input.Location != null) {
                notification.Location = new Point(input.Location.Longitude, input.Location.Latitude);
                notification.Weather = await weather.GetWeather((float)input.Location.Latitude, (float)input.Location.Longitude);
            }
            if (!isBatch)
                return await Save(notification);
            await repo.CreateEntityContainer(notification);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}