using Cosmonaut;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;

namespace trifenix.agro.db.applicationsReference
{
    public class MainDb<T> : IMainDb<T> where T:DocumentBase 
    {

        protected readonly CosmosStoreSettings StoreSettings;
        protected readonly AgroDbArguments MainArgs;

        public ICosmosStore<T> Store { get; private set; }

        

        public MainDb(AgroDbArguments args)
        {
            StoreSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            MainArgs = args;
            Store = new CosmosStore<T>(StoreSettings);
            
        }

        public async Task<string> CreateUpdate(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id)) throw new NonIdException<DocumentBase>(entity);

            var result = await Store.UpsertAsync(entity);
            

            if (!result.IsSuccess) throw result.Exception;

            return result.Entity.Id;
        }

        public async Task<T> GetEntity(string uniqueId) {
            var entity = (T)Activator.CreateInstance(typeof(T));

            return await Store.FindAsync(uniqueId, entity.CosmosEntityName);
        }

        public  IQueryable<T> GetEntities() {

            
            return Store.Query();
        }

        public async Task<long> GetTotalElements() {
            return await Store.QuerySingleAsync<long>("SELECT VALUE COUNT(1) FROM c");
        }
    }



}
