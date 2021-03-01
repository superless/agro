using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones de la orden de aplicacion.
    /// Dentro contiene la validacion de los parametros ingresados y 
    /// el guardado de estas entradas
    /// </summary>
    /// <typeparam name="T">La operacion que se ejecuta</typeparam>
    public class ApplicationOrderOperations<T> : MainOperation<ApplicationOrder, ApplicationOrderInput,T>, IGenericOperation<ApplicationOrder, ApplicationOrderInput> {

        

        public ApplicationOrderOperations(IMainGenericDb<ApplicationOrder> repo,  IAgroSearch<T> search, ICommonAgroQueries commonQueries, IValidatorAttributes<ApplicationOrderInput> validator) : base(repo,  search, validator) {

        }

        public override async Task Validate(ApplicationOrderInput input) {
            await base.Validate(input);

            List<string> errors = new List<string>();
            if (input.OrderType == OrderType.PHENOLOGICAL && !input.IdsPreOrder.Any())
                    errors.Add("Si la orden es fenológica, deben existir preordenes fenologicas asociadas.");

            if (!Enum.IsDefined(typeof(OrderType), input.OrderType))
                throw new ArgumentOutOfRangeException("input","Enum fuera de rango");

            foreach (var doses in input.DosesOrder) {
                bool exists = await existElement.ExistsById<Dose>(doses.IdDoses);
                if (!exists)
                    errors.Add($"No existe dosis con id '{doses.IdDoses}'.");
            }


            foreach (var barrack in input.Barracks) {
                bool exists = await existElement.ExistsById<Barrack>(barrack.IdBarrack);
                if (!exists)
                    errors.Add($"No existe cuartel con id '{barrack.IdBarrack}'.");
                if (barrack.IdNotificationEvents != null && barrack.IdNotificationEvents.Any()) {
                    foreach (var idNotification in barrack.IdNotificationEvents) {
                        bool existsEvent = await existElement.ExistsById<NotificationEvent>(idNotification);
                        if (!existsEvent)
                            errors.Add($"No existe notificacion con id '{idNotification}'.");
                    }
                }
            }


            if (input.StartDate > input.EndDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");

            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }



        public async override Task<ExtPostContainer<string>> SaveInput(ApplicationOrderInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var order = new ApplicationOrder {
                Id = id,
                Barracks = input.Barracks,
                DosesOrder = input.DosesOrder,
                EndDate = input.EndDate,
                StartDate = input.StartDate,
                IdsPreOrder = input.IdsPreOrder,
                Name = input.Name,
                OrderType = input.OrderType,
                Wetting = input.Wetting
            };
            search.DeleteElementsWithRelatedElement(EntityRelated.BARRACK_EVENT, EntityRelated.ORDER, id);
            search.DeleteElementsWithRelatedElement(EntityRelated.DOSES_ORDER, EntityRelated.ORDER, id);
            await SaveDb(order);
            return await SaveSearch(order);


        }

    }

}