using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.email.interfaces;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations.entities.events {

    /// <summary>
    /// Todos las funciones necesarias para interactuar con eventos registrados en el monitoreo.
    /// </summary>
    public class NotificationEventOperations : MainReadOperation<NotificationEvent>, IGenericOperation<NotificationEvent, NotificationEventInput> {
        
        private readonly ICommonQueries commonQueries;
        private readonly IEmail email;
        private readonly IUploadImage uploadImage;
        private readonly IWeatherApi weather;

        public NotificationEventOperations(IMainGenericDb<NotificationEvent> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, IEmail email, IUploadImage uploadImage, IWeatherApi weather, ICommonDbOperations<NotificationEvent> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
            this.email = email;
            this.uploadImage = uploadImage;
            this.weather = weather;
        }

        private async Task<string> ValidaNotification(NotificationEventInput input) {
            string errors = string.Empty;
            if (!string.IsNullOrWhiteSpace(input.Id)) {
                var existsId  = await existElement.ExistsById<NotificationEvent>(input.Id);
                if (!existsId)
                    errors += "No existe notificación a modificar.  ";
            }
            var existsBarrack = await existElement.ExistsById<Barrack>(input.IdBarrack);
            if (!existsBarrack)
                errors += "No existe cuartel    ";
            if (input.EventType == NotificationType.Phenological) {
                var existsPhenologicalEvent = await existElement.ExistsById<PhenologicalEvent>(input.IdPhenologicalEvent);
                if (!existsPhenologicalEvent)
                    errors += "No existe evento fenológico";
            }
            return errors;
        }

        public async Task<ExtPostContainer<string>> Save(NotificationEventInput input) {
            var validaNotification = await ValidaNotification(input);
            if (!string.IsNullOrWhiteSpace(validaNotification))
                throw new Exception(validaNotification);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            NotificationEvent notification = new NotificationEvent {
                Id = id,
                Created = DateTime.Now,
            };
            if (!string.IsNullOrWhiteSpace(input.Id)) {
                var notificationTmp = await Get(input.Id);
                notification = notificationTmp.Result;
            }
            if (input.Lat.HasValue && input.Long.HasValue)
                notification.Weather = await weather.GetWeather(input.Lat.Value, input.Long.Value);
            var picturePath = await uploadImage.UploadImageBase64(input.Base64);
            notification.IdBarrack = input.IdBarrack;
            notification.IdPhenologicalEvent = input.IdPhenologicalEvent;
            notification.NotificationType = input.EventType;
            notification.PicturePath = picturePath;
            notification.Description = input.Description;
            await repo.CreateUpdate(notification);
            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(input.IdBarrack);

            var idSeason = await commonQueries.GetSeasonId(input.IdBarrack);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Created = DateTime.Now,
                    Id = id,
                    RelatedProperties= new Property[]{
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_DESC, Value = input.Description },
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_CODE, Value = specieAbbv },
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_PATH, Value = picturePath}
                    },
                    EntityIndex = (int)EntityRelated.NOTIFICATION_EVENT,
                    RelatedIds = new RelatedId[]{
                      new RelatedId{  EntityIndex=(int)EntityRelated.PHENOLOGICAL_EVENT, EntityId = input.IdPhenologicalEvent},
                      new RelatedId{  EntityIndex=(int)EntityRelated.BARRACK, EntityId = input.IdBarrack},
                      new RelatedId{ EntityIndex=(int)EntityRelated.SEASON, EntityId = idSeason }

                    },
                    RelatedEnumValues = new RelatedEnumValue[]{ 
                        new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.NOTIFICATION_TYPE, Value = (int)input.EventType  }
                    }
                }
            });
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
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }
}