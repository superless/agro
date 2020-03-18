using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders {
    public class PreOrdersOperations : MainOperation<PreOrder, PreOrderInput>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonQueries commonQueries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<PreOrder> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
        }

        //if (!preOrder.BarracksId.Any()) return "Los cuarteles son obligatorios";

        public async Task<ExtPostContainer<string>> Save(PreOrder preOrder) {
            await repo.CreateUpdate(preOrder);
            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(preOrder.BarracksId.First());
            var entity = new EntitySearch {
                Id = preOrder.Id,
                EntityIndex = (int)EntityRelated.PREORDER,
                Created = DateTime.Now,
                RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = preOrder.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        }
                    },
                RelatedEnumValues = new RelatedEnumValue[] {
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.PREORDER_TYPE, Value = (int)preOrder.PreOrderType }
                }
            };
            var entities = preOrder.BarracksId.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.BARRACK, EntityId = s }).ToList();
                //TODO: Revisar
            if (preOrder.PreOrderType == PreOrderType.DEFAULT)
                entities.Add(new RelatedId { EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = preOrder.IdIngredient });
            if (preOrder.PreOrderType == PreOrderType.PHENOLOGICAL)
                entities.Add(new RelatedId { EntityIndex = (int)EntityRelated.ORDER_FOLDER, EntityId = preOrder.OrderFolderId });
            var idSeason = await commonQueries.GetSeasonId(preOrder.BarracksId.First());
            entities.Add(new RelatedId { EntityIndex = (int)EntityRelated.SEASON, EntityId = idSeason });
            entity.RelatedIds = entities.ToArray();
            search.AddElements(new List<EntitySearch> { entity });
            return new ExtPostContainer<string> {
                IdRelated = preOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PreOrderInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var preOrder = new PreOrder {
                Id = id,
                IdIngredient = input.IdIngredient,
                BarracksId = input.BarracksId,
                PreOrderType = input.PreOrderType,
                Name = input.Name,
                OrderFolderId = input.OrderFolderId
            };
            if (!isBatch)
                return await Save(preOrder);
            await repo.CreateEntityContainer(preOrder);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}