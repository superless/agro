using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations.entities.events
{

    /// <summary>
    /// Todos las funciones necesarias para interactuar con eventos registrados en el monitoreo.
    /// </summary>
    public class NotificationEventOperations : MainReadOperation<NotificationEvent>, IGenericOperation<NotificationEvent, NotificationEventInput>
    {
        private readonly ICommonQueries commonQueries;
        private readonly IUploadImage uploadImage;
        private readonly IWeatherApi weather;

        public NotificationEventOperations(IMainGenericDb<NotificationEvent> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries,  IUploadImage uploadImage, IWeatherApi weather) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
            this.uploadImage = uploadImage;
            this.weather = weather;
        }


        private async Task<string> ValidaNotification(NotificationEventInput input) {
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var existsId  = await existElement.ExistsElement<NotificationEvent>(input.Id);

                if (!existsId) return "no existe notificación a modificar";


            }


            var existsBarrack = await existElement.ExistsElement<Barrack>(input.IdBarrack);

            if (!existsBarrack) return "no existe cuartel";


            if (input.EventType == NotificationType.Phenological)
            {
                var existsPhenologicalEvent = await existElement.ExistsElement<PhenologicalEvent>(input.IdPhenologicalEvent);

                if (!existsPhenologicalEvent) return "no existe evento fenológico";

            }

            return string.Empty;



        }

        public async Task<ExtPostContainer<string>> Save(NotificationEventInput input)
        {
            var validaNotification = await ValidaNotification(input);

            if (!string.IsNullOrWhiteSpace(validaNotification)) throw new Exception(validaNotification);


            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            NotificationEvent notification = new NotificationEvent
            {
                Id = id,
                Created = DateTime.Now,


            };

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var notificationTmp = await Get(input.Id);
                notification = notificationTmp.Result;

            }

            if (input.Lat.HasValue && input.Long.HasValue)
            {
                notification.Weather = await weather.GetWeather(input.Lat.Value, input.Long.Value);
            }

            var picturePath = await uploadImage.UploadImageBase64(input.Base64);

            notification.IdBarrack = input.IdBarrack;
            notification.IdPhenologicalEvent = input.IdPhenologicalEvent;
            notification.NotificationType = input.EventType;
            notification.PicturePath = picturePath;
            notification.Description = input.Description;
            await repo.CreateUpdate(notification);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(input.IdBarrack);


            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    RelatedProperties= new Property[]{
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_DESC, Value = input.Description },
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_CODE, Value = specieAbbv },
                       new Property{ PropertyIndex=(int)PropertyRelated.GENERIC_PATH, Value = picturePath}
                    },
                    EntityIndex = (int)EntityRelated.NOTIFICATION,
                    RelatedIds = new RelatedId[]{
                      new RelatedId{  EntityIndex=(int)EntityRelated.PHENOLOGICAL_EVENT, EntityId = input.IdPhenologicalEvent},
                      new RelatedId{  EntityIndex=(int)EntityRelated.BARRACK, EntityId = input.IdBarrack}

                    },
                    RelatedEnumValues = new RelatedEnumValue[]{ 
                        new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.NOTIFICATION_TYPE, Value = (int)input.EventType  }
                    }


                }
            });




            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }
}