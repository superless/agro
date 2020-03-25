using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders {

    public class ApplicationOrderOperations : MainReadOperationName<ApplicationOrder, ApplicationOrderInput>, IGenericOperation<ApplicationOrder, ApplicationOrderInput> {

        private readonly ICommonQueries commonQueries;

        public ApplicationOrderOperations(IMainGenericDb<ApplicationOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<ApplicationOrder> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
        }
        public async Task Remove(string id)
        {

        }
        private async Task<string> ValidaOrder(ApplicationOrderInput input) {
            string errors = string.Empty;
            if (input.OrderType == OrderType.PHENOLOGICAL) {
                if (!input.IdsPhenologicalPreOrder.Any())
                    errors += "Deben existir preordenes fenológicas, cuando la orden de aplicación es de tipo fenológica\r\n";
                else
                    foreach (var preOrderId in input.IdsPhenologicalPreOrder) {
                        bool exists = await existElement.ExistsById<PreOrder>(preOrderId);
                        if (!exists)
                            errors += $"La preorden con id {preOrderId} no existe\r\n";
                    }
            }
            if (!input.DosesOrder.Any())
                errors += "Debe existir al menos una dosis\r\n";
            else
                foreach (var doses in input.DosesOrder) {
                    bool exists = await existElement.ExistsById<Doses>(doses.IdDoses);
                    if (!exists)
                        errors += $"La dosis con id {doses.IdDoses} no existe\r\n";
                }
            if (!input.Barracks.Any())
                errors += "Debe existir al menos un cuartel\r\n";
            else    
                foreach (var barrack in input.Barracks) {
                    bool exists = await existElement.ExistsById<Barrack>(barrack.IdBarrack);
                    if (!exists)
                        errors += $"El cuartel con id {barrack.IdBarrack} no existe\r\n";
                    if (barrack.IdNotificationEvents.Any()) {
                        foreach (var idNotification in barrack.IdNotificationEvents) {
                            bool existsEvent = await existElement.ExistsById<NotificationEvent>(idNotification);
                            if (!existsEvent)
                                errors += $"La notificación con id {idNotification} no existe\r\n";
                        }
                    }
                }
            if (input.InitDate > input.EndDate)
                errors += "La fecha inicial no puede ser mayor a la final\r\n";
            return errors;
        }

        public async Task<ExtPostContainer<string>> Save(ApplicationOrderInput input) {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var valida = await Validate(input);
            if (!valida)
                throw new Exception(string.Format(ErrorMessages.NotValid, "PreOrden"));
            var validaPreOrder = await ValidaOrder(input);
            if (!string.IsNullOrEmpty(validaPreOrder))
                throw new Exception(validaPreOrder);
            var order = new ApplicationOrder {
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
            var entity = new EntitySearch {
                Id = id,
                EntityIndex = (int)EntityRelated.ORDER,
                Created = DateTime.Now,
                RelatedProperties = new Property[] {
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = input.Name },
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION, Value = specieAbbv },
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_WETTING, Value =  $"{input.Wetting}" },
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE, Value = $"{input.InitDate : dd/MM/yyyy}" },
                    new Property { PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE, Value = $"{input.EndDate : dd/MM/yyyy}" }
                },
                RelatedEnumValues = new RelatedEnumValue[] {
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.ORDER_TYPE, Value = (int)input.OrderType }
                }
            };

            var relatedEntities = new List<RelatedId>();


            

            search.DeleteElementsWithRelatedElement(EntityRelated.BARRACK_EVENT, EntityRelated.ORDER, id);


            //TODO : Eliminar antes de agregar
            foreach (var barrack in input.Barracks) {
                var idGuid = Guid.NewGuid().ToString("N");
                var relatedIds = new List<RelatedId>() {
                    new RelatedId{ EntityIndex = (int)EntityRelated.BARRACK, EntityId = barrack.IdBarrack },
                    new RelatedId { EntityIndex = (int)EntityRelated.ORDER, EntityId = id }
                };
                relatedIds.AddRange(barrack.IdNotificationEvents.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.NOTIFICATION_EVENT, EntityId = s }));
                var inputSearch = new EntitySearch {
                    Id = idGuid,
                    EntityIndex = (int)EntityRelated.BARRACK_EVENT,
                    Created = DateTime.Now,
                    References = relatedIds.ToArray()
                };
                search.AddElements(new List<EntitySearch> {
                    inputSearch
                });
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.BARRACK_EVENT, EntityId = idGuid });
            }

            // Eliminar antes de agregar            
            search.DeleteElementsWithRelatedElement(EntityRelated.DOSES_ORDER, EntityRelated.ORDER, id);


            //TODO : Eliminar antes de agregar
            foreach (var doses in input.DosesOrder) {
                var idGuid = Guid.NewGuid().ToString("N");
                var inputSearch = new EntitySearch {
                    Id = idGuid,
                    Created = DateTime.Now,
                    EntityIndex = (int)EntityRelated.DOSES_ORDER,
                    RelatedProperties = new Property[] {
                        new Property{ PropertyIndex = (int)PropertyRelated.GENERIC_QUANTITY_HECTARE,  Value = $"{doses.QuantityByHectare}" }
                    },
                    References = new RelatedId[] {
                        new RelatedId{ EntityIndex=(int)EntityRelated.DOSES, EntityId = doses.IdDoses },
                        new RelatedId { EntityIndex = (int)EntityRelated.ORDER, EntityId = id }
                    }
                };
                search.AddElements(new List<EntitySearch> {
                    inputSearch
                });
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.DOSES_ORDER, EntityId = idGuid });
            }

            if (input.OrderType == OrderType.PHENOLOGICAL)
                relatedEntities.AddRange(input.IdsPhenologicalPreOrder.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.PREORDER, EntityId = s }));
            var idSeason = await commonQueries.GetSeasonId(input.Barracks.First().IdBarrack);
            relatedEntities.Add( new RelatedId { EntityIndex = (int)EntityRelated.SEASON, EntityId = idSeason });
            entity.References = relatedEntities.ToArray();
            search.AddElements(new List<EntitySearch> { entity });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}