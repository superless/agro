using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.external.operations.helper;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external.main
{

    public abstract class MainOperation<T, T_INPUT,T_GEO>  where T : DocumentBase where T_INPUT : InputBase {
        
        protected readonly IMainGenericDb<T> repo;
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch<T_GEO> search;
        protected readonly ICommonDbOperations<T> commonDb;
        
        protected readonly IValidatorAttributes<T_INPUT, T> valida;

        public MainOperation(IMainGenericDb<T> repo, IAgroSearch<T_GEO> search, ICommonDbOperations<T> commonDb, IValidatorAttributes<T_INPUT, T> validator) {
            this.repo = repo;
            
            this.search = search;
            this.commonDb = commonDb;
            this.valida = validator;
        }

        public virtual async Task Validate(T_INPUT input) {
            var result = await valida.Valida(input);

            if (!result.Valid)
            {
                throw new Exception(string.Join(",", result.Messages));
            }

        }

        public async Task<ExtGetContainer<T>> Get(string id) {
            var entity = await repo.GetEntity(id);
            return OperationHelper.GetElement(entity);
        }

        public async Task<ExtGetContainer<List<T>>> GetElements() {
            var entityQuery = repo.GetEntities();
            var entities = await commonDb.TolistAsync(entityQuery);
            return OperationHelper.GetElements(entities);
        }

       
        
    }

}