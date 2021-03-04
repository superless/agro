using Microsoft.Azure.Documents.Spatial;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.upload;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{

    /// <summary>
    /// Todos las funciones necesarias para interactuar con eventos registrados en el monitoreo.
    /// </summary>
    public class NotificationEventOperations<T> : MainOperation<NotificationEvent, NotificationEventInput,T>, IGenericOperation<NotificationEvent, NotificationEventInput> {
        
        private readonly ICommonAgroQueries commonQueries;
        private readonly IEmail email;
        private readonly IUploadImage uploadImage;
        private readonly IWeatherApi weather;

        public NotificationEventOperations(IMainGenericDb<NotificationEvent> repo, IAgroSearch<T> search, ICommonAgroQueries commonQueries, IEmail email, IUploadImage uploadImage, IWeatherApi weather, IValidatorAttributes<NotificationEventInput> validator, ILogger log) : base(repo, search, validator, log)
        {
            this.commonQueries = commonQueries;
            this.email = email;
            this.uploadImage = uploadImage;
            this.weather = weather;
        }

        public override async Task<ExtPostContainer<string>> SaveInput(NotificationEventInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var picturePath = await uploadImage.UploadImageBase64(input.Base64);
            

            NotificationEvent notification = new NotificationEvent {
                Id = id,
                Created = DateTime.Now,
                IdBarrack = input.IdBarrack,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                NotificationType = input.NotificationType,
                PicturePath = picturePath,
                Description = input.Description,
                
            };
            //TODO: Cambiar tipo de dato a GeoSpacial
            #if !CONNECT
            if (input.Location != null) {
                notification.Location = new Point(input.Location.Lng, input.Location.Lat);
                notification.Weather = await weather.GetWeather((float)input.Location.Lat, (float)input.Location.Lng);
            }
            #endif

            await SaveDb(notification);
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

            return await SaveSearch(notification);
        }

        
    }

}