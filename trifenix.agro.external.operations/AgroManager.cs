using Cosmonaut;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.email.interfaces;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.entities;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.agro.validator.operations;
using trifenix.agro.weather.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.external.operations
{



    public class AgroManager<T> : IAgroManager<T> {

        private readonly IDbConnect dbConnect;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly string UserId;
        private readonly bool isBatch;

        public AgroManager(IDbConnect dbConnect, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch<T> searchServiceInstance, string ObjectIdAAD, bool _isBatch) {
            this.dbConnect = dbConnect;
            _email = email;
            _uploadImage = uploadImage;
            _weatherApi = weatherApi;
            Search = searchServiceInstance;
            if (!string.IsNullOrWhiteSpace(ObjectIdAAD))
            {
                UserId = CommonQueries.GetUserIdFromAAD(ObjectIdAAD).Result;
            }
            
            isBatch = _isBatch;
        }

        private IMainGenericDb<T2> GetMainDb<T2>() where T2 : DocumentBase => dbConnect.GetMainDb<T2>();

        private ICommonDbOperations<T2> GetCommonDbOp<T2>() where T2 : DocumentBase => dbConnect.GetCommonDbOp<T2>();

        public ICosmosStore<EntityContainer> BatchStore => dbConnect.BatchStore;

        public IExistElement ExistsElements => dbConnect.ExistsElements(isBatch);

        private ICommonQueries CommonQueries => dbConnect.CommonQueries;

        public IAgroSearch<T> Search { get; }

       

        private IValidator Validators => new Validator(new Dictionary<string, IValidate> { { "ReferenceAttribute", new ReferenceValidation(ExistsElements) }, { "RequiredAttribute", new RequiredValidation() }, { "UniqueAttribute", new UniqueValidation(ExistsElements) } });






        /// <summary>
        /// Repositorio de las actividades de usuario.
        /// </summary>
        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations<T>(GetMainDb<UserActivity>(), ExistsElements, Search, GetCommonDbOp<UserActivity>(), UserId, Validators);



        /// <summary>
        /// Repositorio de sectores
        /// </summary>
        public IGenericOperation<Sector, SectorInput> Sector => new SectorOperations<T>(GetMainDb<Sector>(), ExistsElements, Search, GetCommonDbOp<Sector>(), Validators);



        /// <summary>
        /// Repositorio de marcas
        /// </summary>
        public IGenericOperation<Brand, BrandInput> Brand => new BrandOperations<T>(GetMainDb<Brand>(), ExistsElements, Search, GetCommonDbOp<Brand>(), Validators);


        /// <summary>
        /// Repositorio de parcelas
        /// </summary>
        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new PlotLandOperations<T>(GetMainDb<PlotLand>(), ExistsElements, Search, GetCommonDbOp<PlotLand>(), Validators);


        /// <summary>
        /// Repositorio de especies
        /// </summary>
        public IGenericOperation<Specie, SpecieInput> Specie => new SpecieOperations<T>(GetMainDb<Specie>(), ExistsElements, Search, GetCommonDbOp<Specie>(), Validators);


        /// <summary>
        /// Repositorio de variedades
        /// </summary>
        public IGenericOperation<Variety, VarietyInput> Variety => new VarietyOperations<T>(GetMainDb<Variety>(), ExistsElements, Search, GetCommonDbOp<Variety>(), Validators);


        /// <summary>
        /// Repositorio de aplicaciones
        /// </summary>
        public IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget => new ApplicationTargetOperations<T>(GetMainDb<ApplicationTarget>(), ExistsElements, Search, GetCommonDbOp<ApplicationTarget>(), Validators);


        /// <summary>
        /// Repositorio de eventos fenológicos
        /// </summary>
        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new PhenologicalEventOperations<T>(GetMainDb<PhenologicalEvent>(), ExistsElements, Search, GetCommonDbOp<PhenologicalEvent>(), Validators);

        /// <summary>
        /// Entidad certificadora 
        /// </summary>
        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new CertifiedEntityOperations<T>(GetMainDb<CertifiedEntity>(), ExistsElements, Search, GetCommonDbOp<CertifiedEntity>(), Validators);


        /// <summary>
        /// Categoría de ingredientes.
        /// </summary>
        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new IngredientCategoryOperations<T>(GetMainDb<IngredientCategory>(), ExistsElements, Search, GetCommonDbOp<IngredientCategory>(), Validators);


        /// <summary>
        /// ingredientes
        /// </summary>
        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new IngredientOperations<T> (GetMainDb<Ingredient>(), ExistsElements, Search, GetCommonDbOp<Ingredient>(), Validators);


        /// <summary>
        /// Productos
        /// </summary>
        public IGenericOperation<Product, ProductInput> Product => new ProductOperations<T>(GetMainDb<Product>(), ExistsElements, Search, Dose, GetCommonDbOp<Product>(), CommonQueries, Validators);

        /// <summary>
        /// Dosis
        /// </summary>
        public IGenericOperation<Dose, DosesInput> Dose => new DosesOperations<T>(GetMainDb<Dose>(), ExistsElements, Search, GetCommonDbOp<Dose>(), CommonQueries, Validators);


        /// <summary>
        /// Roles
        /// </summary>
        public IGenericOperation<Role, RoleInput> Role => new RoleOperations<T>(GetMainDb<Role>(), ExistsElements, Search, GetCommonDbOp<Role>(), Validators);


        /// <summary>
        /// Puesto de trabajoi
        /// </summary>
        public IGenericOperation<Job, JobInput> Job => new JobOperations<T>(GetMainDb<Job>(), ExistsElements, Search, GetCommonDbOp<Job>(), Validators);

        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations<T>(GetMainDb<UserApplicator>(), ExistsElements, Search, dbConnect.GraphApi , GetCommonDbOp<UserApplicator>(), Validators);

        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new NebulizerOperations<T>(GetMainDb<Nebulizer>(), ExistsElements, Search, GetCommonDbOp<Nebulizer>(), Validators);
        
        public IGenericOperation<Tractor, TractorInput> Tractor => new TractorOperations<T>(GetMainDb<Tractor>(), ExistsElements, Search, GetCommonDbOp<Tractor>(), Validators);

        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new BusinessNameOperations<T>(GetMainDb<BusinessName>(), ExistsElements, Search, GetCommonDbOp<BusinessName>(), Validators);

        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations<T>(GetMainDb<CostCenter>(), ExistsElements, Search, GetCommonDbOp<CostCenter>(), Validators);

        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations<T>(GetMainDb<Season>(), ExistsElements, Search, GetCommonDbOp<Season>(), Validators);

        public IGenericOperation<Rootstock, RootstockInput> Rootstock => new RootstockOperations<T>(GetMainDb<Rootstock>(), ExistsElements, Search, GetCommonDbOp<Rootstock>(), Validators);

        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations<T>(GetMainDb<OrderFolder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<OrderFolder>(), Validators);


        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations<T>(GetMainDb<Barrack>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<Barrack>(), Validators);

        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations<T>(GetMainDb<NotificationEvent>(), ExistsElements, Search, CommonQueries, _email, _uploadImage, _weatherApi, GetCommonDbOp<NotificationEvent>(), Validators);

        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations<T>(GetMainDb<PreOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<PreOrder>(), Validators);

        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations<T>(GetMainDb<ApplicationOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<ApplicationOrder>(), Validators);

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations<T>(GetMainDb<ExecutionOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<ExecutionOrder>(), Validators);

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations<T>(GetMainDb<ExecutionOrderStatus>(), ExistsElements, Search, GetCommonDbOp<ExecutionOrderStatus>(), Validators);
        
        public IGenericOperation<Comment, CommentInput> Comments => new CommentOperation<T>(GetMainDb<Comment>(), ExistsElements, Search, GetCommonDbOp<Comment>(), Validators);

        public dynamic GetOperationByInputType(Type InputType) {
            var operationsProps = typeof(IAgroManager<GeographyPoint>).GetProperties().Where(prop => prop.PropertyType.Name.StartsWith("IGenericOperation`2")).ToList();
            var genProp = operationsProps.FirstOrDefault(prop => prop.PropertyType.GenericTypeArguments[1].Equals(InputType));
            return (dynamic)genProp.GetValue(this);
        }

        public dynamic GetOperationByDbType(Type DbType) {
            var operationsProps = typeof(IAgroManager<GeographyPoint>).GetProperties().Where(prop => prop.PropertyType.Name.StartsWith("IGenericOperation`2")).ToList();
            var genProp = operationsProps.FirstOrDefault(prop => prop.PropertyType.GenericTypeArguments[0].Equals(DbType));
            return (dynamic)genProp?.GetValue(this);
        }

    }

}