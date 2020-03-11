using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.email.interfaces;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.agro.microsoftgraph.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations
{
    public class AgroManager : IAgroManager {

        private readonly AgroDbArguments Arguments;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly IAgroSearch _searchServiceInstance;
        private readonly string UserId;

        public AgroManager(AgroDbArguments arguments, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch searchServiceInstance, string ObjectIdAAD) {
            Arguments = arguments;
            _email = email;
            _uploadImage = uploadImage;
            _weatherApi = weatherApi;
            _searchServiceInstance = searchServiceInstance;
            UserId = CommonQueries.GetUserIdFromAAD(ObjectIdAAD).Result;
        }

        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations(new MainGenericDb<UserActivity>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<UserActivity>(), UserId);

        public IExistElement ExistsElements => new CosmosExistElement(Arguments);

        public ICommonQueries CommonQueries => new CommonQueries(Arguments);

        public IGenericOperation<Sector, SectorInput> Sector => new SectorOperations(new MainGenericDb<Sector>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Sector>());
        
        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new PlotLandOperations(new MainGenericDb<PlotLand>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<PlotLand>());

        public IGenericOperation<Specie, SpecieInput> Specie => new SpecieOperations(new MainGenericDb<Specie>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Specie>());

        public IGenericOperation<Variety, VarietyInput> Variety => new VarietyOperations(new MainGenericDb<Variety>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Variety>());

        public IGenericOperation<ApplicationTarget, TargetInput> ApplicationTarget => new ApplicationTargetOperations(new MainGenericDb<ApplicationTarget>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<ApplicationTarget>());

        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new PhenologicalEventOperations(new MainGenericDb<PhenologicalEvent>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<PhenologicalEvent>());

        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new CertifiedEntityOperations(new MainGenericDb<CertifiedEntity>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<CertifiedEntity>());

        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new IngredientCategoryOperations(new MainGenericDb<IngredientCategory>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<IngredientCategory>());

        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new IngredientOperations(new MainGenericDb<Ingredient>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Ingredient>());

        public IGenericOperation<Product, ProductInput> Product => new ProductOperations(new MainGenericDb<Product>(Arguments), ExistsElements, _searchServiceInstance, Doses, new CommonDbOperations<Product>());

        public IGenericOperation<Doses, DosesInput> Doses => new DosesOperations(new MainGenericDb<Doses>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Doses>());

        public IGenericOperation<Role, RoleInput> Role => new RoleOperations(new MainGenericDb<Role>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Role>());

        public IGenericOperation<Job, JobInput> Job => new JobOperations(new MainGenericDb<Job>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Job>());

        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations(new MainGenericDb<UserApplicator>(Arguments), ExistsElements, _searchServiceInstance, new GraphApi(Arguments), new CommonDbOperations<UserApplicator>());

        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new NebulizerOperations(new MainGenericDb<Nebulizer>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Nebulizer>());
        
        public IGenericOperation<Tractor, TractorInput> Tractor => new TractorOperations(new MainGenericDb<Tractor>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Tractor>());

        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new BusinessNameOperations(new MainGenericDb<BusinessName>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<BusinessName>());

        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations(new MainGenericDb<CostCenter>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<CostCenter>());

        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations(new MainGenericDb<Season>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Season>());

        public IGenericOperation<Rootstock, RootStockInput> Rootstock => new RootstockOperations(new MainGenericDb<Rootstock>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<Rootstock>());

        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations(new MainGenericDb<OrderFolder>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, new CommonDbOperations<OrderFolder>());

        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations(new MainGenericDb<Barrack>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, new CommonDbOperations<Barrack>());

        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations(new MainGenericDb<NotificationEvent>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, _email, _uploadImage, _weatherApi, new CommonDbOperations<NotificationEvent>());

        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations(new MainGenericDb<PreOrder>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, new CommonDbOperations<PreOrder>());

        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations(new MainGenericDb<ApplicationOrder>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, new CommonDbOperations<ApplicationOrder>());

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations(new MainGenericDb<ExecutionOrder>(Arguments), ExistsElements, _searchServiceInstance, CommonQueries, new CommonDbOperations<ExecutionOrder>());

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations(new MainGenericDb<ExecutionOrderStatus>(Arguments), ExistsElements, _searchServiceInstance, new CommonDbOperations<ExecutionOrderStatus>());

    }

}