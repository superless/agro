using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;

namespace trifenix.agro.db
{
    public abstract class MainDb<T> where T:DocumentBase
    {

        protected readonly CosmosStoreSettings StoreSettings;
        protected readonly AgroDbArguments MainArgs;
        protected readonly ICosmosStore<T> Store;

        public MainDb(AgroDbArguments args)
        {
            StoreSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            MainArgs = args;
            Store = new CosmosStore<T>(StoreSettings);
            
        }

        protected async Task<string> CreateUpdate(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id)) throw new NonIdException<DocumentBase>(entity);

            var result = await Store.UpsertAsync(entity);

            if (!result.IsSuccess) throw result.Exception;

            return result.Entity.Id;
        }

        protected async Task<T> GetEntity(string uniqueId) {
            var entity = (T)Activator.CreateInstance(typeof(T));
            return await Store.FindAsync(uniqueId, entity.CosmosEntityName);
        }

        protected  IQueryable<T> GetEntities() {
            return Store.Query();
        }
    }
}
