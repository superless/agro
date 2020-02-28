using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ApplicationOrderOperations : MainReadOperationName<ApplicationOrder, ApplicationOrderInput>, IGenericOperation<ApplicationOrder, ApplicationOrderInput>
    {
        private readonly ICommonQueries commonQueries;

        public ApplicationOrderOperations(IMainGenericDb<ApplicationOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
        }


        private async Task<string> ValidaOrder(ApplicationOrderInput input) {
            if (input.OrderType == OrderType.PHENOLOGICAL)
            {
                if (!input.IdsPhenologicalPreOrder.Any()) return "Deben existir preordenes fenológicas, cuando la orden de aplicación es de tipo fenológica";

                foreach (var preOrderId in input.IdsPhenologicalPreOrder)
                {
                    var exists = await existElement.ExistsElement<PreOrder>(preOrderId);

                    if (!exists) return $"la preorden con id {preOrderId} no existe";
                }

            }


            if (!input.DosesOrder.Any()) return "Debe existir al menos un producto";

            foreach (var doses in input.DosesOrder)
            {
                var exists = await existElement.ExistsElement<Doses>(doses.IdDoses);
                if (!exists) return $"la dosis con id {doses.IdDoses} no existe";

            }

            if (!input.Barracks.Any()) return "Debe existir al menos un cuartel";

            foreach (var barrack in input.Barracks)
            {
                var exists = await existElement.ExistsElement<Barrack>(barrack.IdBarrack);
                if (!exists) return $"el cuartel con id {barrack.IdBarrack} no existe";
                if (barrack.IdEvents.Any())
                {
                    foreach (var idNotif in barrack.IdEvents)
                    {
                        var existsEvent = await existElement.ExistsElement<NotificationEvent>(idNotif);

                        if (!existsEvent) return $"la notificación con id {idNotif} no existe";
                    }

                }

            }

            if (input.InitDate > input.EndDate) return "La fecha inicial no puede ser mayor a la final";

            return string.Empty;
        }

        public async Task<ExtPostContainer<string>> Save(ApplicationOrderInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");


            var valida = await Validate(input);

            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, "PreOrden"));

            var validaPreOrder = await ValidaOrder(input);

            if (!string.IsNullOrWhiteSpace(validaPreOrder)) throw new Exception(validaPreOrder);

            var order = new ApplicationOrder
            {
                Id = id,
                Barracks = input.Barracks,
                DosesOrder = input.DosesOrder,
                EndDate = input.EndDate,
                InitDate = input.InitDate,
                IdsPhenologicalPreOrder = input.IdsPhenologicalPreOrder,
                Name = input.Name,
                OrderType = input.OrderType,
                Wetting = input.Wetting
            };



            await repo.CreateUpdate(order);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(input.Barracks.First().IdBarrack);


            var entity = new EntitySearch
            {
                Id = id,
                EntityIndex = (int)EntityRelated.APPLICATION_ORDER,
                Created = DateTime.Now,
                RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        },
                        new Property{ 
                            PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE,
                            Value = $"{input.InitDate : dd/MM/yyyy}"
                        },
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE,
                            Value = $"{input.EndDate : dd/MM/yyyy}"
                        }
                    },
                RelatedEnumValues = new RelatedEnumValue[] {
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.ORDER_TYPE, Value = (int)input.OrderType }
                }
            };

            var entitiesForBarrack = input.Barracks.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.BARRACK, EntityId = s.IdBarrack }).ToList();

            var entitiesForEvents = input.Barracks.SelectMany(s => s.IdEvents).Distinct().Select(s => new RelatedId { EntityIndex = (int)EntityRelated.NOTIFICATION, EntityId = s});

            var entitiesForDoses = input.DosesOrder.Select(s => s.IdDoses).Distinct().Select(s => new RelatedId { EntityIndex = (int)EntityRelated.DOSES, EntityId = s });

            var entities = new List<RelatedId>();

            entities.AddRange(entitiesForBarrack);

            entities.AddRange(entitiesForEvents);

            entities.AddRange(entitiesForDoses);
            


            if (input.OrderType == OrderType.PHENOLOGICAL)
            {
                entities.AddRange(input.IdsPhenologicalPreOrder.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.PREORDER, EntityId = s }));
            }

            var idSeason = await commonQueries.GetSeasonId(input.Barracks.First().IdBarrack);
            entities.Add( new RelatedId { EntityIndex = (int)EntityRelated.SEASON, EntityId = idSeason });


            entity.RelatedIds = entities.ToArray();



            search.AddElements(new List<EntitySearch> {
                entity
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