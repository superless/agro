using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro.model;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.orders
{
    public class PreOrdersOperations : MainOperation<PreOrder, PreOrderInput>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonQueries commonQueries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonQueries commonQueries, ICommonDbOperations<PreOrder> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.commonQueries = commonQueries;
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        //if (!preOrder.BarracksId.Any()) return "Los cuarteles son obligatorios";

        public async Task<ExtPostContainer<string>> Save(PreOrder preOrder) {
            await repo.CreateUpdate(preOrder);
            search.AddDocument(preOrder);

            return new ExtPostContainer<string> {
                IdRelated = preOrder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(PreOrderInput input, bool isBatch) {
            await Validate(input);
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