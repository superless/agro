using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations {

   

    public abstract class MainBaseOperation<T> where T : DocumentBase {
        
        protected readonly IMainGenericDb<T> repo;
        private readonly ICommonDbOperations<T> commonDb;

        public MainBaseOperation(IMainGenericDb<T> repo, ICommonDbOperations<T> commonDb) {
            this.repo = repo;
            this.commonDb = commonDb;
        }

        public async Task<ExtGetContainer<T>> Get(string id) {
            var entity = await repo.GetEntity(id);
            return OperationHelper.GetElement(entity);
        }

        public async Task<ExtGetContainer<List<T>>> GetElements()
        {
            var entityQuery = repo.GetEntities();
            var entities = await commonDb.TolistAsync(entityQuery);
            return OperationHelper.GetElements(entities);
        }

    }

    public abstract class MainReadOperation<T> : MainBaseOperation<T> where T : DocumentBase {
        
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch search;

        public MainReadOperation(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<T> commonDb) : base(repo, commonDb) {
            this.existElement = existElement;
            this.search = search;
        }


        
    }

}