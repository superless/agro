using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.attr;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.model;
using trifenix.agro.util;

namespace trifenix.agro.db.applicationsReference
{

    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase {

        public ICosmosStore<T> Store { get; }
        public ICosmosStore<EntityContainer> BatchStore { get; }

        public MainGenericDb(AgroDbArguments args) {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            Store = new CosmosStore<T>(storeSettings);
            BatchStore = new CosmosStore<EntityContainer>(storeSettings);
        }

        private EntityContainer GetContainer(T entity) => new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = entity };

        private async Task<T> SetClientId(T entity, PropertyInfo prop_ClientId) {
            dynamic castedEntity = entity;
            var autoNumericSearchAttribute = prop_ClientId.GetAttribute<AutoNumericSearchAttribute>();
            bool numerateByDependence = autoNumericSearchAttribute.Dependant.HasValue;
            if (numerateByDependence) {
                var prop_referenceToIndependent = entity.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(ReferenceSearchAttribute)) && prop.GetAttribute<ReferenceSearchAttribute>().Index == (int)autoNumericSearchAttribute.Dependant);
                var idIndependent = (string)prop_referenceToIndependent?.GetValue(entity);
                var query = $"SELECT * FROM c WHERE c.{prop_referenceToIndependent.Name} = '{idIndependent}'";
                var dependentElements = (IEnumerable<DocumentBase<int>>)await Store.QueryMultipleAsync(query);
                castedEntity.ClientId = dependentElements.Max(element => (int?)element.ClientId) ?? 0 + 1;
            } else
                castedEntity.ClientId = Store.Query().Max(element => ((DocumentBase<int>)(object)element).ClientId) + 1;
            return castedEntity;
        }

        //public async Task updateClientId(T entity) {
        //    dynamic castedEntity = entity;
        //    var prop_ClientId = entity.GetType().GetProperty("ClientId");
        //    if(prop_ClientId != null) {
        //        var autoNumericSearchAttribute = prop_ClientId.GetAttribute<AutoNumericSearchAttribute>();
        //        bool numerateByDependence = autoNumericSearchAttribute.Dependant.HasValue;
        //        if (numerateByDependence) {
        //            var prop_referenceToIndependent = entity.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(ReferenceSearchAttribute)) && prop.GetAttribute<ReferenceSearchAttribute>().Index == (int)autoNumericSearchAttribute.Dependant);
        //            var idIndependent = (string)prop_referenceToIndependent?.GetValue(entity);
        //            var dependentElements = await Store.QueryMultipleAsync<DocumentBase<int>>($"SELECT * FROM c WHERE c.{prop_referenceToIndependent.Name} = '{idIndependent}'");
        //            castedEntity.ClientId = dependentElements.Max(element => element.ClientId) + 1;
        //        } else
        //            castedEntity.ClientId = Store.Query().Max(element => ((DocumentBase<int>)(object)element).ClientId) + 1;
        //        await Store.UpsertAsync(castedEntity);
        //    }
        //}

        public async Task<string> CreateUpdate(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var alreadyExists = (await Store.QuerySingleAsync($"SELECT * FROM c WHERE c.Id = '{entity.Id}'")) != null;
            var prop_ClientId = entity.GetType().GetProperty("ClientId");
            if (!alreadyExists && prop_ClientId != null)
                entity = await SetClientId(entity, prop_ClientId);
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