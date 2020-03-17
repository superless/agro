using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations {

    public abstract class MainOperation<T, T_INPUT> where T : DocumentBase where T_INPUT : InputBase {
        
        protected readonly IMainGenericDb<T> repo;
        protected readonly IExistElement existElement;
        protected readonly IAgroSearch search;
        private readonly ICommonDbOperations<T> commonDb;

        public MainOperation(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<T> commonDb) {
            this.repo = repo;
            this.existElement = existElement;
            this.search = search;
            this.commonDb = commonDb;
        }

        public virtual async Task Validate(T_INPUT input, bool isBatch) {
            if (!string.IsNullOrWhiteSpace(input.Id)) {  //PUT
                var existsId = await existElement.ExistsById<T>(input.Id, isBatch);
                if (!existsId)
                    throw new Validation_Exception { ErrorMessages = new List<string> { $"No existe {typeof(T).Name} con id '{input.Id}'." } };
            }
            ValidateRequiredProperties(input);
            await ValidateReferenceProperties(input, isBatch);
            await ValidateUniqueProperties(input, isBatch);

        }

        private void ValidateRequiredProperties(T_INPUT input) {
            List<string> errors = new List<string>();
            var RequiredProperties = typeof(T_INPUT).GetProperties().Where(prop => prop.GetCustomAttributes(true).Any(attr => attr is RequiredAttribute)).ToList();
            RequiredProperties.ForEach(prop => {
                var value = prop.GetValue(input);
                bool hasValue = prop.PropertyType.Equals(typeof(string)) ? !string.IsNullOrWhiteSpace((string)value) : value != null && (value.GetType() is System.Collections.IList?((IEnumerable<object>)value).Any():true);
                if (!hasValue)
                    errors.Add($"{prop.Name} es un atributo requerido.");
            });
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        private async Task ValidateReferenceProperties(T_INPUT input, bool isBatch) {
            List<string> errors = new List<string>();
            Type entityOfReference;
            bool existsEntityReferenced;
            var ReferenceProperties = typeof(T_INPUT).GetProperties().Where(prop => prop.GetCustomAttributes(true).Any(attr => attr is Reference)).ToList();
            foreach (var prop in ReferenceProperties) {
                entityOfReference = ((Reference)prop.GetCustomAttributes(typeof(Reference), true).FirstOrDefault()).entityOfReference;
                var value = prop.GetValue(input);
                List<string> references = value.GetType() is System.Collections.IList ? new List<string>((string[])value) : new List<string> { (string)value };
                foreach(var id in references.Where(reference => !string.IsNullOrWhiteSpace(reference))) {
                    existsEntityReferenced = await (Task<bool>)typeof(CosmosExistElement).GetMethod("ExistsById").MakeGenericMethod(entityOfReference).Invoke(existElement, new object[] { id, isBatch });
                    if (!existsEntityReferenced)
                        errors.Add($"No existe {entityOfReference.Name} con id '{id}'.");
                }
            }
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

        private async Task ValidateUniqueProperties(T_INPUT input, bool isBatch) {
            List<string> errors = new List<string>();
            var UniqueProperties = typeof(T_INPUT).GetProperties().Where(prop => prop.GetCustomAttributes(true).Any(attr => attr is Unique)).ToList();
            foreach (var prop in UniqueProperties) {
                var value = (string)prop.GetValue(input);
                //Si input.Id es null, significa que es un POST, en caso contrario es un PUT. La validacion siguiente considera esto.
                bool existsProperty = await existElement.ExistsWithPropertyValue<T>(prop.Name, value, input.Id, isBatch);
                if (existsProperty)
                    errors.Add($"{prop.Name} debe ser un atributo unico. Ya existe otro {typeof(T).Name} con valor '{value}' en esta propiedad.");
            }
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
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