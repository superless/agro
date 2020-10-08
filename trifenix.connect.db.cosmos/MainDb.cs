using Cosmonaut;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;

namespace trifenix.connect.db.cosmos
{

    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase {

        public ICosmosStore<T> Store { get; }
        public ICosmosStore<EntityContainer> BatchStore { get; }

        public MainGenericDb(CosmosDbArguments args) {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            Store = new CosmosStore<T>(storeSettings);
            BatchStore = new CosmosStore<EntityContainer>(storeSettings);
        }

        private EntityContainer GetContainer(T entity) => new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = entity };

     

       

        public async Task<string> CreateUpdate(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var result = await Store.UpsertAsync(entity);
            if (!result.IsSuccess)
                throw result.Exception;
            return result.Entity.Id;
        }

        public async Task<string> CreateEntityContainer(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var result = await BatchStore.UpsertAsync(GetContainer(entity));
            if (!result.IsSuccess)
                throw result.Exception;
            return result.Entity.Id;
        }

        public async Task<T> GetEntity(string id) => await Store.FindAsync(id);
       

        public async Task DeleteEntity(string id) {
            
            await Store.RemoveByIdAsync(id, new Microsoft.Azure.Documents.Client.RequestOptions { 
                PartitionKey = new Microsoft.Azure.Documents.PartitionKey("CosmosEntityName")
            });
        } 

        public IQueryable<T> GetEntities() => Store.Query();

    }

}