using Cosmonaut;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
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
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.microsoftgraph.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.agro.validator.operations;
using trifenix.agro.weather.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.external.operations {


    /// <summary>
    /// Enlaces a base de datos, para las distintas operaicones
    /// </summary>
    public class DbConnect : IDbConnect
    {
        public DbConnect(AgroDbArguments arguments)
        {
            Arguments = arguments;
        }

        // argumentos de base de datos
        public AgroDbArguments Arguments { get; }


        // batchstore usado para realizar operaciones en batch en la base de datos.
        public ICosmosStore<EntityContainer> BatchStore =>   new CosmosStore<EntityContainer>(new CosmosStoreSettings(Arguments.NameDb, Arguments.EndPointUrl, Arguments.PrimaryKey));

        // consultas comunes.
        public ICommonQueries CommonQueries => new CommonQueries(Arguments);


        // Elementos en existencia.
        public IExistElement ExistsElements(bool isBatch) => isBatch? (IExistElement) new BatchExistsElements(Arguments) : new CosmosExistElement(Arguments);


        // Operaciones comunes en la base de datgos
        public ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase => new CommonDbOperations<T>();


        // Operaciones comunes en la base de datos (CRUD).
        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase
        {
            return new MainGenericDb<T>(Arguments);
        }

        // 
        public IGraphApi GraphApi => new GraphApi(Arguments);
    }



    public class AgroManager : IAgroManager<GeographyPoint> {

        private readonly IDbConnect dbConnect;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly string UserId;
        private readonly bool isBatch;

        public AgroManager(IDbConnect dbConnect, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch<GeographyPoint> searchServiceInstance, string ObjectIdAAD, bool _isBatch) {
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

        private IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase => dbConnect.GetMainDb<T>();

        private ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase => dbConnect.GetCommonDbOp<T>();

        public ICosmosStore<EntityContainer> BatchStore => dbConnect.BatchStore;

        public IExistElement ExistsElements => dbConnect.ExistsElements(isBatch);

        private ICommonQueries CommonQueries => dbConnect.CommonQueries;

        public IAgroSearch<GeographyPoint> Search { get; }

       

        private IValidator Validators => new Validator(new Dictionary<string, IValidate> { { "ReferenceAttribute", new ReferenceValidation(ExistsElements) }, { "RequiredAttribute", new RequiredValidation() }, { "UniqueAttribute", new UniqueValidation(ExistsElements) } });

        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations(GetMainDb<UserActivity>(), ExistsElements, Search, GetCommonDbOp<UserActivity>(), UserId, Validators);

        public IGenericOperation<Sector, SectorInput> Sector => new SectorOperations(GetMainDb<Sector>(), ExistsElements, Search, GetCommonDbOp<Sector>(), Validators);

        public IGenericOperation<Brand, BrandInput> Brand => new BrandOperations(GetMainDb<Brand>(), ExistsElements, Search, GetCommonDbOp<Brand>(), Validators);

        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new PlotLandOperations(GetMainDb<PlotLand>(), ExistsElements, Search, GetCommonDbOp<PlotLand>(), Validators);

        public IGenericOperation<Specie, SpecieInput> Specie => new SpecieOperations(GetMainDb<Specie>(), ExistsElements, Search, GetCommonDbOp<Specie>(), Validators);

        public IGenericOperation<Variety, VarietyInput> Variety => new VarietyOperations(GetMainDb<Variety>(), ExistsElements, Search, GetCommonDbOp<Variety>(), Validators);

        public IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget => new ApplicationTargetOperations(GetMainDb<ApplicationTarget>(), ExistsElements, Search, GetCommonDbOp<ApplicationTarget>(), Validators);

        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new PhenologicalEventOperations(GetMainDb<PhenologicalEvent>(), ExistsElements, Search, GetCommonDbOp<PhenologicalEvent>(), Validators);

        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new CertifiedEntityOperations(GetMainDb<CertifiedEntity>(), ExistsElements, Search, GetCommonDbOp<CertifiedEntity>(), Validators);

        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new IngredientCategoryOperations(GetMainDb<IngredientCategory>(), ExistsElements, Search, GetCommonDbOp<IngredientCategory>(), Validators);

        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new IngredientOperations(GetMainDb<Ingredient>(), ExistsElements, Search, GetCommonDbOp<Ingredient>(), Validators);

        public IGenericOperation<Product, ProductInput> Product => new ProductOperations(GetMainDb<Product>(), ExistsElements, Search, Dose, GetCommonDbOp<Product>(), CommonQueries, Validators);

        public IGenericOperation<Dose, DosesInput> Dose => new DosesOperations(GetMainDb<Dose>(), ExistsElements, Search, GetCommonDbOp<Dose>(), CommonQueries, Validators);

        public IGenericOperation<Role, RoleInput> Role => new RoleOperations(GetMainDb<Role>(), ExistsElements, Search, GetCommonDbOp<Role>(), Validators);

        public IGenericOperation<Job, JobInput> Job => new JobOperations(GetMainDb<Job>(), ExistsElements, Search, GetCommonDbOp<Job>(), Validators);

        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations(GetMainDb<UserApplicator>(), ExistsElements, Search, dbConnect.GraphApi , GetCommonDbOp<UserApplicator>(), Validators);

        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new NebulizerOperations(GetMainDb<Nebulizer>(), ExistsElements, Search, GetCommonDbOp<Nebulizer>(), Validators);
        
        public IGenericOperation<Tractor, TractorInput> Tractor => new TractorOperations(GetMainDb<Tractor>(), ExistsElements, Search, GetCommonDbOp<Tractor>(), Validators);

        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new BusinessNameOperations(GetMainDb<BusinessName>(), ExistsElements, Search, GetCommonDbOp<BusinessName>(), Validators);

        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations(GetMainDb<CostCenter>(), ExistsElements, Search, GetCommonDbOp<CostCenter>(), Validators);

        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations(GetMainDb<Season>(), ExistsElements, Search, GetCommonDbOp<Season>(), Validators);

        public IGenericOperation<Rootstock, RootstockInput> Rootstock => new RootstockOperations(GetMainDb<Rootstock>(), ExistsElements, Search, GetCommonDbOp<Rootstock>(), Validators);

        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations(GetMainDb<OrderFolder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<OrderFolder>(), Validators);

        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations(GetMainDb<Barrack>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<Barrack>(), Validators);

        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations(GetMainDb<NotificationEvent>(), ExistsElements, Search, CommonQueries, _email, _uploadImage, _weatherApi, GetCommonDbOp<NotificationEvent>(), Validators);

        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations(GetMainDb<PreOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<PreOrder>(), Validators);

        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations(GetMainDb<ApplicationOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<ApplicationOrder>(), Validators);

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations(GetMainDb<ExecutionOrder>(), ExistsElements, Search, CommonQueries, GetCommonDbOp<ExecutionOrder>(), Validators);

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations(GetMainDb<ExecutionOrderStatus>(), ExistsElements, Search, GetCommonDbOp<ExecutionOrderStatus>(), Validators);
        
        public IGenericOperation<Comment, CommentInput> Comments => new CommentOperation(GetMainDb<Comment>(), ExistsElements, Search, GetCommonDbOp<Comment>(), Validators);

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