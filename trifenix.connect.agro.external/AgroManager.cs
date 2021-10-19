using Microsoft.Extensions.Logging;
using Microsoft.Spatial;
using System;
using System.Linq;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.orders;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.model;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.input;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.upload;
using trifenix.connect.model;

namespace trifenix.agro.external
{

    public class AgroManager<T> : IAgroManager<T>
    {

        private readonly IDbAgroConnect dbConnect;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly ILogger log;
        private readonly string UserId;

        public AgroManager(IDbAgroConnect dbConnect, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch<T> searchServiceInstance, string ObjectIdAAD, ILogger log)
        {
            this.dbConnect = dbConnect;
            _email = email;
            _uploadImage = uploadImage;
            _weatherApi = weatherApi;
            Search = searchServiceInstance;
            this.log = log;
            if (!string.IsNullOrWhiteSpace(ObjectIdAAD))
            {
                UserId = CommonQueries.GetUserIdFromAAD(ObjectIdAAD).Result;
            }
        }

        private IMainGenericDb<T2> GetMainDb<T2>() where T2 : DocumentDb => dbConnect.GetMainDb<T2>();

        

        public IValidatorAttributes<T_INPUT> GetValidators<T_INPUT, T_DB>() where T_INPUT : InputBase where T_DB : DocumentDb => dbConnect.GetValidator<T_INPUT, T_DB>();

        private ICommonAgroQueries CommonQueries => dbConnect.CommonQueries;

        public IAgroSearch<T> Search { get; }

        /// <summary>
        /// Repositorio de las actividades de usuario.
        /// </summary>
        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations<T>(GetMainDb<UserActivity>(), Search, UserId, GetValidators<UserActivityInput, UserActivity>(), log);

        /// <summary>
        /// Repositorio de sectores
        /// </summary>
        public IGenericOperation<Sector, SectorInput> Sector => new MainOperation<Sector, SectorInput, T>(GetMainDb<Sector>(), Search, GetValidators<SectorInput, Sector>(), log);

        /// <summary>
        /// Repositorio de marcas
        /// </summary>
        public IGenericOperation<Brand, BrandInput> Brand => new MainOperation<Brand, BrandInput, T>(GetMainDb<Brand>(), Search, GetValidators<BrandInput, Brand>(), log);

        /// <summary>
        /// Repositorio de parcelas
        /// </summary>
        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new MainOperation<PlotLand, PlotLandInput, T>(GetMainDb<PlotLand>(), Search, GetValidators<PlotLandInput, PlotLand>(), log);

        /// <summary>
        /// Repositorio de especies
        /// </summary>
        public IGenericOperation<Specie, SpecieInput> Specie => new MainOperation<Specie, SpecieInput, T>(GetMainDb<Specie>(), Search, GetValidators<SpecieInput, Specie>(), log);

        /// <summary>
        /// Repositorio de variedades
        /// </summary>
        public IGenericOperation<Variety, VarietyInput> Variety => new MainOperation<Variety, VarietyInput, T>(GetMainDb<Variety>(), Search,  GetValidators<VarietyInput, Variety>(), log);

        /// <summary>
        /// Repositrio de bodegas
        /// </summary>
        public IGenericOperation<Warehouse, WarehouseInput> Warehouse => new MainOperation<Warehouse, WarehouseInput, T>(GetMainDb<Warehouse>(), Search,  GetValidators<WarehouseInput, Warehouse>(), log);

        /// <summary>
        /// Repositorio de aplicaciones
        /// </summary>
        public IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget => new MainOperation<ApplicationTarget, ApplicationTargetInput, T>(GetMainDb<ApplicationTarget>(), Search,  GetValidators<ApplicationTargetInput, ApplicationTarget>(), log);

        /// <summary>
        /// Repositorio de eventos fenológicos
        /// </summary>
        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new MainOperation<PhenologicalEvent, PhenologicalEventInput, T>(GetMainDb<PhenologicalEvent>(), Search, GetValidators<PhenologicalEventInput, PhenologicalEvent>(), log);

        /// <summary>
        /// Entidad certificadora 
        /// </summary>
        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new MainOperation<CertifiedEntity, CertifiedEntityInput, T>(GetMainDb<CertifiedEntity>(), Search, GetValidators<CertifiedEntityInput, CertifiedEntity>(), log);

        /// <summary>
        /// Categoría de ingredientes.
        /// </summary>
        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new MainOperation<IngredientCategory, IngredientCategoryInput, T>(GetMainDb<IngredientCategory>(), Search, GetValidators<IngredientCategoryInput, IngredientCategory>(), log);

        /// <summary>
        /// ingredientes
        /// </summary>
        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new MainOperation<Ingredient, IngredientInput, T>(GetMainDb<Ingredient>(), Search,  GetValidators<IngredientInput, Ingredient>(), log);

        /// <summary>
        /// Productos
        /// </summary>
        public IGenericOperation<Product, ProductInput> Product => new ProductOperations<T>(GetMainDb<Product>(), Search, Dose, CommonQueries, GetValidators<ProductInput, Product>(), log);

        /// <summary>
        /// Dosis
        /// </summary>
        public IGenericOperation<Dose, DosesInput> Dose => new DosesOperations<T>(dbConnect.GetDbExistsElements, GetMainDb<Dose>(), Search,  CommonQueries, GetValidators<DosesInput, Dose>(), log);

        /// <summary>
        /// Documento de bodega
        /// </summary>
        public IGenericOperation<WarehouseDocument, WarehouseDocumentInput> WarehouseDocument => new WarehouseDocumentOperations<T>(dbConnect.GetDbExistsElements, GetMainDb<WarehouseDocument>(), Search,  CommonQueries, GetValidators<WarehouseDocumentInput, WarehouseDocument>(), log);

        /// <summary>
        /// Roles
        /// </summary>
        public IGenericOperation<Role, RoleInput> Role => new MainOperation<Role, RoleInput, T>(GetMainDb<Role>(), Search, GetValidators<RoleInput, Role>(), log);

        /// <summary>
        /// Puesto de trabajoi
        /// </summary>
        public IGenericOperation<Job, JobInput> Job => new MainOperation<Job, JobInput, T>(GetMainDb<Job>(), Search,  GetValidators<JobInput, Job>(), log);

        /// <summary>
        /// Usuario
        /// </summary>
        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations<T>(GetMainDb<UserApplicator>(), Search, dbConnect.GraphApi, GetValidators<UserApplicatorInput, UserApplicator>(), log);

        /// <summary>
        /// Nebulizador
        /// </summary>
        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new MainOperation<Nebulizer, NebulizerInput, T>(GetMainDb<Nebulizer>(), Search, GetValidators<NebulizerInput, Nebulizer>(), log);

        /// <summary>
        /// Tractor
        /// </summary>
        public IGenericOperation<Tractor, TractorInput> Tractor => new MainOperation<Tractor, TractorInput, T>(GetMainDb<Tractor>(), Search, GetValidators<TractorInput, Tractor>(), log);

        /// <summary>
        /// Empresa
        /// </summary>
        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new MainOperation<BusinessName, BusinessNameInput, T>(GetMainDb<BusinessName>(), Search, GetValidators<BusinessNameInput, BusinessName>(), log);

        /// <summary>
        /// Centro de costos
        /// </summary>
        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations<T>(GetMainDb<CostCenter>(), Search,  CommonQueries, GetValidators<CostCenterInput, CostCenter>(), log);

        /// <summary>
        /// Temporada
        /// </summary>
        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations<T>(GetMainDb<Season>(), Search, CommonQueries, GetValidators<SeasonInput, Season>(), log);

        /// <summary>
        /// Portainjerto
        /// </summary>
        public IGenericOperation<Rootstock, RootstockInput> Rootstock => new MainOperation<Rootstock, RootstockInput, T>(GetMainDb<Rootstock>(), Search,  GetValidators<RootstockInput, Rootstock>(), log);

        /// <summary>
        /// OrderFolder
        /// </summary>
        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations<T>(GetMainDb<OrderFolder>(), Search, CommonQueries,  GetValidators<OrderFolderInput, OrderFolder>(), log);

        /// <summary>
        /// Cuartel
        /// </summary>
        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations<T>(GetMainDb<Barrack>(), Search, CommonQueries, GetValidators<BarrackInput, Barrack>(), log);

        /// <summary>
        /// Notificacion de evento
        /// </summary>
        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations<T>(GetMainDb<NotificationEvent>(), Search, CommonQueries, _email, _uploadImage, _weatherApi, GetValidators<NotificationEventInput, NotificationEvent>(), log);

        /// <summary>
        /// Pre Orden
        /// </summary>
        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations<T>(GetMainDb<PreOrder>(), Search, CommonQueries, GetValidators<PreOrderInput, PreOrder>(), log);

        /// <summary>
        /// Orden de aplicacion
        /// </summary>
        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations<T>(GetMainDb<ApplicationOrder>(), Search, CommonQueries, GetValidators<ApplicationOrderInput, ApplicationOrder>(), log);

        /// <summary>
        /// Orden de ejecucion
        /// </summary>
        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations<T>(GetMainDb<ExecutionOrder>(), Search, CommonQueries, GetValidators<ExecutionOrderInput, ExecutionOrder>(), log);

        /// <summary>
        /// Estado de orden de ejecucion
        /// </summary>
        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations<T>(GetMainDb<ExecutionOrderStatus>(), Search, GetValidators<ExecutionOrderStatusInput, ExecutionOrderStatus>(), log);

        /// <summary>
        /// Comentario
        /// </summary>
        public IGenericOperation<Comment, CommentInput> Comments => new MainOperation<Comment, CommentInput, T>(GetMainDb<Comment>(), Search, GetValidators<CommentInput, Comment>(), log);

        public dynamic GetOperationByInputType(Type InputType)
        {
            var operationsProps = typeof(IAgroManager<GeographyPoint>).GetProperties().Where(prop => prop.PropertyType.Name.StartsWith("IGenericOperation`2")).ToList();
            var genProp = operationsProps.FirstOrDefault(prop => prop.PropertyType.GenericTypeArguments[1].Equals(InputType));
            return (dynamic)genProp.GetValue(this);
        }

        public dynamic GetOperationByDbType(Type DbType)
        {
            var operationsProps = typeof(IAgroManager<GeographyPoint>).GetProperties().Where(prop => prop.PropertyType.Name.StartsWith("IGenericOperation`2")).ToList();
            var genProp = operationsProps.FirstOrDefault(prop => prop.PropertyType.GenericTypeArguments[0].Equals(DbType));
            return (dynamic)genProp?.GetValue(this);
        }

    }
}