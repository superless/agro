using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm_attributes;
using trifenix.connect.util;

namespace trifenix.agro.external.operations
{

    public abstract class MainOperation<T, T_INPUT,T_GEO>  where T : DocumentBase where T_INPUT : InputBase {
        
        protected readonly IMainGenericDb<T> repo;
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch<T_GEO> search;
        private readonly ICommonDbOperations<T> commonDb;
        
        private IValidatorAttributes<T_INPUT, T> valida;

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

        public async Task RenewClientIds() {
            await repo.RenewClientIds();
        }

        
    }

}