using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using trifenix.agro.attr;
using trifenix.agro.db;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations
{

    public abstract class MainOperation<T, T_INPUT> where T : DocumentBase where T_INPUT : InputBase {
        
        protected readonly IMainGenericDb<T> repo;
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch search;
        private readonly ICommonDbOperations<T> commonDb;
        private readonly IValidator validator;

        public MainOperation(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<T> commonDb, IValidator validator) {
            this.repo = repo;
            this.existElement = existElement;
            this.search = search;
            this.commonDb = commonDb;
            this.validator = validator;
        }

        public virtual async Task Validate(T_INPUT input) {
            if (!string.IsNullOrWhiteSpace(input.Id)) {  //PUT
                var existsId = await existElement.ExistsById<T>(input.Id);
                if (!existsId)
                    throw new Validation_Exception { ErrorMessages = new List<string> { $"No existe {typeof(T).Name} con id '{input.Id}'." } };
            }
            await validator.ValidateRecursively<RequiredAttribute>(input);
            await validator.ValidateRecursively<ReferenceAttribute>(input);
            await validator.ValidateRecursively<UniqueAttribute>(input);
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