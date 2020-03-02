using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations
{
    public abstract class MainFullReadOperation<T> : MainBaseOperation<T> where T : DocumentBase
    {
        private readonly IMainDb<T> fullRepo;
        private readonly ICommonDbOperations<T> commonDb;

        public MainFullReadOperation(IMainDb<T> repo, ICommonDbOperations<T> commonDb) : base(repo)
        {
            fullRepo = repo;
            this.commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<T>>> GetElements()
        {
            var entityQuery = fullRepo.GetEntities();
            var entities = await commonDb.TolistAsync(entityQuery);
            return OperationHelper.GetElements(entities);
        }
    }

    public abstract class MainBaseOperation<T> where T : DocumentBase {
        protected readonly IMainGenericDb<T> repo;

        public MainBaseOperation(IMainGenericDb<T> repo)
        {
            this.repo = repo;
        }
        public async Task<ExtGetContainer<T>> Get(string id)
        {
            var sector = await repo.GetEntity(id);
            return OperationHelper.GetElement(sector);
        }

    }

    public abstract class MainReadOperation<T> : MainBaseOperation<T> where T : DocumentBase
    {
        
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch search;

        public MainReadOperation(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search) : base(repo)
        {
            
            this.existElement = existElement;
            this.search = search;
        }
        
    }
}
