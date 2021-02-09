using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.external.operations.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;
using trifenix.connect.util;

namespace trifenix.connect.agro.external.main
{

    /// <summary>
    /// Clase Principal de operación en sistema agroFenix
    /// Clase encargada de las operaciones en la base de datos de persistencia y busqueda
    /// además de la sincronización de ambas bases de datos.
    /// El modelo se sustenta en los atributos de las clases, los que originan la creación de documentos en bases de datos de busqueda
    /// bajo un modelo entitySearch, que permite indexar de mejor manera la busqueda.
    /// Este modelo es ampliable, todos sus métodos esenciales son virtual.
    /// </summary>
    /// <typeparam name="T">Entidad de base de datos</typeparam>
    /// <typeparam name="T_INPUT">Entidad usada como entrada de base de datos</typeparam>
    /// <typeparam name="T_GEO">Tipo Geo de la base de datos de busqueda</typeparam>
    public class MainOperation<T, T_INPUT,T_GEO> : IGenericOperation<T, T_INPUT> where T : DocumentBase where T_INPUT : InputBase {
        
        // modelo de base de datos
        protected readonly IMainGenericDb<T> repo;

        // operaciones de busqueda de entidades en la base de datos
        protected readonly IExistElement existElement;

        // operaciones en la base de datos de busqueda
        protected readonly IAgroSearch<T_GEO> search;

        // operaciones comunes de base de datos
        protected readonly ICommonDbOperations<T> commonDb;
        
        // operaciones de validación.
        protected readonly IValidatorAttributes<T_INPUT> valida;

        // mapper que permite convertir desde un input a un modelo de base de datos.
        // esto solo se logra en input que tienen los mismos campos que el de la base de datos.
        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<T_INPUT,T>();
        });

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="repo">Repositorio de base de datos de la entidad a operar, por ejemplo producto</param>
        /// <param name="search">repositorio de base de datos de busqueda bajo el modelo de entitySearch</param>
        /// <param name="commonDb">operaciones de conversión de IQueriable a listas</param>
        /// <param name="validator">Validador de entidades input</param>
        public MainOperation(IMainGenericDb<T> repo, IAgroSearch<T_GEO> search, ICommonDbOperations<T> commonDb, IValidatorAttributes<T_INPUT> validator) {
            this.repo = repo;
            this.search = search;
            this.commonDb = commonDb;
            this.valida = validator;
            this.existElement = validator.GetExistElement();
        }

        /// <summary>
        /// Valida un elemento input.
        /// </summary>
        /// <param name="input">elemento de ingreso</param>
        /// <returns>Excepción si no es válido</returns>
        public virtual async Task Validate(T_INPUT input) {
            var result = await valida.Valida(input);

            if (!result.Valid)
            {
                throw new Exception(string.Join(",", result.Messages));
            }
            

        }

        /// <summary>
        /// Obtiene una entidad desde la base de datos.
        /// </summary>
        /// <param name="id">identificador de la entidad</param>
        /// <returns>Resultado Get</returns>
        public async Task<ExtGetContainer<T>> Get(string id) {
            var entity = await repo.GetEntity(id);
            return OperationHelper.GetElement(entity);
        }


       

        /// <summary>
        /// Colección de consultas
        /// </summary>
        public virtual Dictionary<string, List<string>> Queried { get; set; } = new Dictionary<string, List<string>>();


        /// <summary>
        /// Guarda una entidad en la base de datos de persitencia
        /// </summary>
        /// <param name="item">elemento a guardar</param>
        /// <returns></returns>
        public virtual async Task<ExtPostContainer<string>> SaveDb(T item)
        {
            // crea o actualiza en la base de datos de persistencia
            await repo.CreateUpdate(item);

            // registra el elemento
            AddToQueried(nameof(MainOperation<T, T_INPUT, T_GEO>.SaveDb), JsonConvert.SerializeObject(item));

            // retorna post ok
            return new ExtPostContainer<string>
            {
                IdRelated = item.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        /// <summary>
        /// Guarda una entidad en la base de datos de busqueda.
        /// </summary>
        /// <param name="entity">entidad a guardar</param>
        /// <returns>Resultado</returns>
        public virtual async Task<ExtPostContainer<string>> SaveSearch(T entity)
        {
            
            // añade una nueva entidad
            await Task.Run(() => search.AddDocument(entity));

            AddToQueried(nameof(MainOperation<T, T_INPUT, T_GEO>.SaveSearch), search.Queried["AddDocument"]);

            return new ExtPostContainer<string>
            {
                IdRelated = entity.Id,
                MessageResult = ExtMessageResult.Ok
            };

        }


        /// <summary>
        /// añade una consulta al registro de operaciones
        /// </summary>
        /// <param name="methodName">nombre del método a registrar</param>
        /// <param name="query">consulta a ingresar</param>
        private void AddToQueried(string methodName, string query)
        {
            if (!Queried.ContainsKey(methodName))
            {
                Queried.Add(methodName, new List<string> { query });
            }
            else
            {
                Queried[methodName].Add(query);
            }
        }

        /// <summary>
        /// añade una consulta al registro de operaciones
        /// </summary>
        /// <param name="methodName">nombre del método a registrar</param>
        /// <param name="queries">Consultas a agregar</param>
        private void AddToQueried(string methodName, List<string> queries)
        {
            if (!Queried.ContainsKey(methodName))
            {
                Queried.Add(methodName, queries);
            }
            else
            {
                Queried[methodName].AddRange(queries);
            }
        }


        /// <summary>
        /// Método por defecto para guardar un elemento en ambas bases de datos.
        /// una base de datos de persistencia con el modelo de clases tradicional
        /// y en la base de datos de busqueda bajo el modelo de entitySearch
        /// </summary>
        /// <param name="input">elemento enviado por usuario</param>
        /// <returns></returns>
        public virtual async Task<ExtPostContainer<string>> SaveInput(T_INPUT input)
        {
            // válida el elemento, según los atributos de su clase.
            await Validate(input);

            // si el elemento no tiene una id, se asigna una nueva
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            input.Id = id;

            // usamos automapper, para mapear directamente desde un input a un modelo de base de datos.
            var mapperModel = config.CreateMapper();
            var entityModel = mapperModel.Map<T>(input);


            // guarda en la base de datos
            var resultDb = await SaveDb(entityModel);

            // si guarda ok en la base de datos
            if (resultDb.MessageResult == ExtMessageResult.Ok)
            {
                // guarda en la base de busqueda como entitySearch
                return await SaveSearch(entityModel);
            }
            return resultDb;
        }

        /// <summary>
        /// Método por defecto para eliminar en ambas bases de datos
        /// </summary>
        /// <param name="id">idenficador del elemento a eliminar</param>
        /// <returns></returns>
        public async virtual Task Remove(string id)
        {
            // elimina desde la base de datos de persistencia.
            await repo.DeleteEntity(id);

            // obtiene el atributo que debería identificar el elemento en el search.
            var attr = Mdm.Reflection.Attributes.GetAttributes<ReferenceSearchHeaderAttribute>(typeof(T)).FirstOrDefault();
            if (attr != null)
            {
                // borrando elemento desde la base de datos de busqueda.
                search.DeleteEntity((EntityRelated)attr.Index, id);
            }
        }

    }

}