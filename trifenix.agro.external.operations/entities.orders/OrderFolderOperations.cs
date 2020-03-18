using System;
using System.Collections.Generic;
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
    public class OrderFolderOperations : MainOperation<OrderFolder, OrderFolderInput>, IGenericOperation<OrderFolder, OrderFolderInput> {
        private readonly ICommonQueries commonQueries;

        public OrderFolderOperations(IMainGenericDb<OrderFolder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<OrderFolder> commonDb) : base(repo, existElement, search, commonDb) {
            this.commonQueries = commonQueries;
        }

        public async Task<ExtPostContainer<string>> Save(OrderFolder orderFolder) {
            await repo.CreateUpdate(orderFolder);
            var specieAbbv = await commonQueries.GetSpecieAbbreviation(orderFolder.IdSpecie);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = orderFolder.Id,
                    EntityIndex = (int)EntityRelated.ORDER_FOLDER,
                    Created = DateTime.Now,
                    RelatedIds = new RelatedId[]{
                        new RelatedId{ EntityIndex = (int)EntityRelated.TARGET, EntityId = orderFolder.IdApplicationTarget  },
                        new RelatedId{ EntityIndex = (int)EntityRelated.CATEGORY_INGREDIENT, EntityId = orderFolder.IdIngredientCategory  },
                        new RelatedId{ EntityIndex = (int)EntityRelated.PHENOLOGICAL_EVENT, EntityId = orderFolder.IdPhenologicalEvent },
                        new RelatedId{ EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = orderFolder.IdIngredient },
                        new RelatedId{ EntityIndex = (int)EntityRelated.PREORDER, EntityId = orderFolder.IdSpecie}
                    },
                    RelatedProperties = new Property[]{
                        new Property{
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        }
                    }

                }
            });
            return new ExtPostContainer<string> {
                IdRelated = orderFolder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(OrderFolderInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var orderFolder = new OrderFolder {
                Id = id,
                IdApplicationTarget = input.IdApplicationTarget,
                IdIngredientCategory = input.IdIngredientCategory,
                IdIngredient = input.IdIngredient,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                IdSpecie = input.IdSpecie
            };
            if (!isBatch)
                return await Save(orderFolder);
            await repo.CreateEntityContainer(orderFolder);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }
        
    }

}