using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.orders {

    public class ApplicationOrderOperations : MainOperation<ApplicationOrder, ApplicationOrderInput>, IGenericOperation<ApplicationOrder, ApplicationOrderInput> {

        private readonly ICommonQueries commonQueries;

        public ApplicationOrderOperations(IMainGenericDb<ApplicationOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<ApplicationOrder> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.commonQueries = commonQueries;
        }

        public async Task Remove(string id) { }

        public override async Task Validate(ApplicationOrderInput applicationOrderInput) {
            await base.Validate(applicationOrderInput);
            List<string> errors = new List<string>();
            if (applicationOrderInput.OrderType == OrderType.PHENOLOGICAL && !applicationOrderInput.IdsPhenologicalPreOrder.Any())
                    errors.Add("Si la orden es fenológica, deben existir preordenes fenologicas asociadas.");
            foreach (var doses in applicationOrderInput.DosesOrder) {
                bool exists = await existElement.ExistsById<Dose>(doses.IdDoses);
                if (!exists)
                    errors.Add($"No existe dosis con id '{doses.IdDoses}'.");
            }
            foreach (var barrack in applicationOrderInput.Barracks) {
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
            if (applicationOrderInput.StartDate > applicationOrderInput.EndDate)
                errors.Add("La fecha inicial no puede ser mayor a la final.");
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }


        public async Task<ExtPostContainer<string>> Save(ApplicationOrder applicationOrder) {
            await repo.CreateUpdate(applicationOrder);
            var relatedEntities = new List<RelatedId>();
            // Eliminar antes de agregar
            search.DeleteElementsWithRelatedElement(EntityRelated.BARRACK_EVENT, EntityRelated.ORDER, applicationOrder.Id);
            //TODO : Eliminar antes de agregar
            foreach (var barrack in applicationOrder.Barracks) {
                var idGuid = Guid.NewGuid().ToString("N");
                var relatedIds = new List<RelatedId>() {
                    new RelatedId{ EntityIndex = (int)EntityRelated.BARRACK, EntityId = barrack.IdBarrack },
                    new RelatedId { EntityIndex = (int)EntityRelated.ORDER, EntityId = applicationOrder.Id }
                };
                relatedIds.AddRange(barrack.IdNotificationEvents.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.NOTIFICATION_EVENT, EntityId = s }));
                search.AddElements(new List<EntitySearch> {
                    new EntitySearch {
                        Id = idGuid,
                        EntityIndex = (int)EntityRelated.BARRACK_EVENT,
                        Created = DateTime.Now,
                        RelatedIds = relatedIds.ToArray()
                    }
                });
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.BARRACK_EVENT, EntityId = idGuid });
            }
            // Eliminar antes de agregar            
            search.DeleteElementsWithRelatedElement(EntityRelated.DOSES_ORDER, EntityRelated.ORDER, applicationOrder.Id);
            //TODO : Eliminar antes de agregar
            foreach (var doses in applicationOrder.DosesOrder) {
                var idGuid = Guid.NewGuid().ToString("N");
                search.AddElements(new List<EntitySearch> {
                    new EntitySearch {
                        Id = idGuid,
                        Created = DateTime.Now,
                        EntityIndex = (int)EntityRelated.DOSES_ORDER,
                        RelatedProperties = new Property[] {
                            new Property { PropertyIndex = (int)PropertyRelated.GENERIC_QUANTITY_HECTARE,  Value = $"{doses.QuantityByHectare}" }
                        },
                        RelatedIds = new RelatedId[] {
                            new RelatedId { EntityIndex=(int)EntityRelated.DOSES, EntityId = doses.IdDoses },
                            new RelatedId { EntityIndex = (int)EntityRelated.ORDER, EntityId = applicationOrder.Id }
                        }
                    }
                });
                relatedEntities.Add(new RelatedId { EntityIndex = (int)EntityRelated.DOSES_ORDER, EntityId = idGuid });
            }

            if (applicationOrder.OrderType == OrderType.PHENOLOGICAL)
                relatedEntities.AddRange(applicationOrder.IdsPreOrder.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.PREORDER, EntityId = s }));
            var idSeason = await commonQueries.GetSeasonId(applicationOrder.Barracks.First().IdBarrack);
            relatedEntities.Add( new RelatedId { EntityIndex = (int)EntityRelated.SEASON, EntityId = idSeason });

            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(applicationOrder.Barracks.First().IdBarrack);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = applicationOrder.Id,
                    EntityIndex = (int)EntityRelated.ORDER,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = applicationOrder.Name },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION, Value = specieAbbv },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_WETTING, Value =  $"{applicationOrder.Wetting}" },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_START_DATE, Value = $"{applicationOrder.StartDate : dd/MM/yyyy}" },
                        new Property { PropertyIndex = (int)PropertyRelated.GENERIC_END_DATE, Value = $"{applicationOrder.EndDate : dd/MM/yyyy}" }
                    },
                    RelatedIds = relatedEntities.ToArray(),
                    RelatedEnumValues = new RelatedEnumValue[] { new RelatedEnumValue { EnumerationIndex = (int)EnumerationRelated.ORDER_TYPE, Value = (int)applicationOrder.OrderType } }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = applicationOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(ApplicationOrderInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var order = new ApplicationOrder {
                Id = id,
                Barracks = input.Barracks,
                DosesOrder = input.DosesOrder,
                EndDate = input.EndDate,
                StartDate = input.StartDate,
                IdsPreOrder = input.IdsPhenologicalPreOrder,
                Name = input.Name,
                OrderType = input.OrderType,
                Wetting = input.Wetting
            };
            if (!isBatch)
                return await Save(order);
            await repo.CreateEntityContainer(order);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}