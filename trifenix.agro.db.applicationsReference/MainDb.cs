using Cosmonaut;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference {

    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase {

        public ICosmosStore<T> Store { get; }
        public ICosmosStore<EntityContainer> BatchStore { get; }

        public MainGenericDb(AgroDbArguments args) {
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
        //{
        //    var entity = (T)Activator.CreateInstance(typeof(T));
        //    return await Store.FindAsync(uniqueId, entity.CosmosEntityName);
        //}

        public async Task DeleteEntity(string id) {
            //var entity = (T)Activator.CreateInstance(typeof(T));
            //await Store.RemoveByIdAsync(id, entity.CosmosEntityName);
            await Store.RemoveByIdAsync(id);
        } 

        public IQueryable<T> GetEntities() => Store.Query();

    }

}