using Cosmonaut;
using Microsoft.Spatial;
using System;
using System.Linq;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.connect.agro.external;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.upload;

namespace trifenix.agro.external.operations
{



    public class AgroManager<T> : IAgroManager<T> {

        private readonly IDbAgroConnect dbConnect;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly string UserId;
        private readonly bool isBatch;

        

        public AgroManager(IDbAgroConnect dbConnect, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch<T> searchServiceInstance, string ObjectIdAAD, bool _isBatch) {
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

        

        public IValidatorAttributes<T_INPUT, T_DB> GetValidators<T_INPUT, T_DB>() where T_INPUT : InputBase where T_DB : DocumentBase => dbConnect.GetValidator<T_INPUT, T_DB>(isBatch);


        private ICommonQueries CommonQueries => dbConnect.CommonQueries;

        public IAgroSearch<T> Search { get; }

       
         

        






        /// <summary>
        /// Repositorio de las actividades de usuario.
        /// </summary>
        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations<T>(GetMainDb<UserActivity>(), Search, GetCommonDbOp<UserActivity>(), UserId, GetValidators<UserActivityInput, UserActivity>());



        /// <summary>
        /// Repositorio de sectores
        /// </summary>
        public IGenericOperation<Sector, SectorInput> Sector => new SectorOperations<T>(GetMainDb<Sector>(), Search, GetCommonDbOp<Sector>(), GetValidators<SectorInput, Sector>());



        /// <summary>
        /// Repositorio de marcas
        /// </summary>
        public IGenericOperation<Brand, BrandInput> Brand => new BrandOperations<T>(GetMainDb<Brand>(), Search, GetCommonDbOp<Brand>(), GetValidators<BrandInput, Brand>());


        /// <summary>
        /// Repositorio de parcelas
        /// </summary>
        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new PlotLandOperations<T>(GetMainDb<PlotLand>(), Search, GetCommonDbOp<PlotLand>(), GetValidators<PlotLandInput, PlotLand>());


        /// <summary>
        /// Repositorio de especies
        /// </summary>
        public IGenericOperation<Specie, SpecieInput> Specie => new SpecieOperations<T>(GetMainDb<Specie>(), Search, GetCommonDbOp<Specie>(), GetValidators<SpecieInput, Specie>());


        /// <summary>
        /// Repositorio de variedades
        /// </summary>
        public IGenericOperation<Variety, VarietyInput> Variety => new VarietyOperations<T>(GetMainDb<Variety>(), Search, GetCommonDbOp<Variety>(), GetValidators<VarietyInput, Variety>());


        /// <summary>
        /// Repositorio de aplicaciones
        /// </summary>
        public IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget => new ApplicationTargetOperations<T>(GetMainDb<ApplicationTarget>(), Search, GetCommonDbOp<ApplicationTarget>(), GetValidators<ApplicationTargetInput, ApplicationTarget>());


        /// <summary>
        /// Repositorio de eventos fenológicos
        /// </summary>
        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new PhenologicalEventOperations<T>(GetMainDb<PhenologicalEvent>(), Search, GetCommonDbOp<PhenologicalEvent>(), GetValidators<PhenologicalEventInput, PhenologicalEvent>());

        /// <summary>
        /// Entidad certificadora 
        /// </summary>
        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new CertifiedEntityOperations<T>(GetMainDb<CertifiedEntity>(), Search, GetCommonDbOp<CertifiedEntity>(), GetValidators<CertifiedEntityInput, CertifiedEntity>());


        /// <summary>
        /// Categoría de ingredientes.
        /// </summary>
        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new IngredientCategoryOperations<T>(GetMainDb<IngredientCategory>(), Search, GetCommonDbOp<IngredientCategory>(), GetValidators<IngredientCategoryInput, IngredientCategory>());


        /// <summary>
        /// ingredientes
        /// </summary>
        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new IngredientOperations<T> (GetMainDb<Ingredient>(), Search, GetCommonDbOp<Ingredient>(), GetValidators<IngredientInput, Ingredient>());


        /// <summary>
        /// Productos
        /// </summary>
        public IGenericOperation<Product, ProductInput> Product => new ProductOperations<T>(GetMainDb<Product>(), Search, Dose, GetCommonDbOp<Product>(), CommonQueries, GetValidators<ProductInput, Product>());

        /// <summary>
        /// Dosis
        /// </summary>
        public IGenericOperation<Dose, DosesInput> Dose => new DosesOperations<T>(dbConnect.GetDbExistsElements, GetMainDb<Dose>(), Search, GetCommonDbOp<Dose>(), CommonQueries, GetValidators<DosesInput, Dose>());


        /// <summary>
        /// Roles
        /// </summary>
        public IGenericOperation<Role, RoleInput> Role => new RoleOperations<T>(GetMainDb<Role>(), Search, GetCommonDbOp<Role>(), GetValidators<RoleInput, Role>());


        /// <summary>
        /// Puesto de trabajoi
        /// </summary>
        public IGenericOperation<Job, JobInput> Job => new JobOperations<T>(GetMainDb<Job>(), Search, GetCommonDbOp<Job>(), GetValidators<JobInput, Job>());

        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations<T>(GetMainDb<UserApplicator>(), Search, dbConnect.GraphApi , GetCommonDbOp<UserApplicator>(), GetValidators<UserApplicatorInput, UserApplicator>());

        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new NebulizerOperations<T>(GetMainDb<Nebulizer>(), Search, GetCommonDbOp<Nebulizer>(), GetValidators<NebulizerInput, Nebulizer>());
        
        public IGenericOperation<Tractor, TractorInput> Tractor => new TractorOperations<T>(GetMainDb<Tractor>(), Search, GetCommonDbOp<Tractor>(), GetValidators<TractorInput, Tractor>());

        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new BusinessNameOperations<T>(GetMainDb<BusinessName>(), Search, GetCommonDbOp<BusinessName>(), GetValidators<BusinessNameInput, BusinessName>());

        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations<T>(GetMainDb<CostCenter>(), Search, GetCommonDbOp<CostCenter>(), GetValidators<CostCenterInput, CostCenter>());

        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations<T>(GetMainDb<Season>(), Search, GetCommonDbOp<Season>(), GetValidators<SeasonInput, Season>());

        public IGenericOperation<Rootstock, RootstockInput> Rootstock => new RootstockOperations<T>(GetMainDb<Rootstock>(), Search, GetCommonDbOp<Rootstock>(), GetValidators<RootstockInput, Rootstock>());

        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations<T>(GetMainDb<OrderFolder>(), Search, CommonQueries, GetCommonDbOp<OrderFolder>(), GetValidators<OrderFolderInput, OrderFolder>());


        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations<T>(GetMainDb<Barrack>(), Search, CommonQueries, GetCommonDbOp<Barrack>(), GetValidators<BarrackInput, Barrack>());

        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations<T>(GetMainDb<NotificationEvent>(), Search, CommonQueries, _email, _uploadImage, _weatherApi, GetCommonDbOp<NotificationEvent>(), GetValidators<NotificationEventInput, NotificationEvent>());

        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations<T>(GetMainDb<PreOrder>(), Search, CommonQueries, GetCommonDbOp<PreOrder>(), GetValidators<PreOrderInput, PreOrder>());

        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations<T>(GetMainDb<ApplicationOrder>(), Search, CommonQueries, GetCommonDbOp<ApplicationOrder>(), GetValidators<ApplicationOrderInput, ApplicationOrder>());

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations<T>(GetMainDb<ExecutionOrder>(), Search, CommonQueries, GetCommonDbOp<ExecutionOrder>(), GetValidators<ExecutionOrderInput, ExecutionOrder>());

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations<T>(GetMainDb<ExecutionOrderStatus>(), Search, GetCommonDbOp<ExecutionOrderStatus>(), GetValidators<ExecutionOrderStatusInput, ExecutionOrderStatus>());
        
        public IGenericOperation<Comment, CommentInput> Comments => new CommentOperation<T>(GetMainDb<Comment>(), Search, GetCommonDbOp<Comment>(), GetValidators<CommentInput, Comment>());

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