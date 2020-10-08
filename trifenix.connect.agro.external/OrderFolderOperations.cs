using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class OrderFolderOperations<T> : MainOperation<OrderFolder, OrderFolderInput,T>, IGenericOperation<OrderFolder, OrderFolderInput> {
        

        public OrderFolderOperations(IMainGenericDb<OrderFolder> repo, IAgroSearch<T> search, ICommonQueries commonQueries, ICommonDbOperations<OrderFolder> commonDb, IValidatorAttributes<OrderFolderInput, OrderFolder> validator) : base(repo, search, commonDb, validator) {
            
        }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(OrderFolder orderFolder) {
            await repo.CreateUpdate(orderFolder);
            search.AddDocument(orderFolder);
            
            return new ExtPostContainer<string> {
                IdRelated = orderFolder.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(OrderFolderInput input, bool isBatch) {
            await Validate(input);
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