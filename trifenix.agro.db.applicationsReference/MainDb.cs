using Cosmonaut;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;

namespace trifenix.agro.db.applicationsReference
{

    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase
    {

        public MainGenericDb(AgroDbArguments args)
        {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            MainArgs = args;
            Store = new CosmosStore<T>(storeSettings);

        }


        public ICosmosStore<T> Store  {get ;}

        public AgroDbArguments MainArgs {get;}

        public async Task<string> CreateUpdate(T entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var result = await Store.UpsertAsync(entity);
            if (!result.IsSuccess)
                throw result.Exception;
            return result.Entity.Id;
        }

        public async Task<T> GetEntity(string uniqueId)
        {
            var entity = (T)Activator.CreateInstance(typeof(T));
            return await Store.FindAsync(uniqueId, entity.CosmosEntityName);
        }
    }
    public class MainDb<T> : MainGenericDb<T>, IMainDb<T> where T: DocumentBase
    {
        public MainDb(AgroDbArguments args) : base(args)
        {   
        }

        public  IQueryable<T> GetEntities() {
            return Store.Query();
        }
    }
}
