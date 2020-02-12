using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities.core;
using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.agro.external.operations.entities.orders.args;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations {
    public class AgroManager : IAgroManager {

        private readonly IAgroRepository _repository;
        private readonly string _idSeason;
        private readonly IUploadImage _uploadImage;
        private readonly IGraphApi _graphApi;
        private readonly IWeatherApi _weatherApi;
        private readonly IAgroSearch _searchServiceInstance;

        public AgroManager(IAgroRepository repository, string idSeason, IUploadImage uploadImage, IGraphApi graphApi, IWeatherApi weatherApi, IAgroSearch searchServiceInstance) {
            _repository = repository;
            _idSeason = idSeason;
            _uploadImage = uploadImage;
            _graphApi = graphApi;
            _weatherApi = weatherApi;
            _searchServiceInstance = searchServiceInstance;
        }

        public string IdSeason { get => _idSeason; }

        public IPhenologicalOperations PhenologicalEvents => new PhenologicalEventOperations(_repository.PhenologicalEvents, new CommonDbOperations<PhenologicalEvent>());

        public IApplicationTargetOperations ApplicationTargets => new ApplicationTargetOperations(_repository.Targets, new CommonDbOperations<ApplicationTarget>());

        public IJobOperations Jobs => new JobOperations(_repository.Jobs, new CommonDbOperations<Job>());

        public IRoleOperations Roles => new RoleOperations(_repository.Roles, new CommonDbOperations<Role>());

        public IUserOperations Users => new UserOperations(_repository.Users, _graphApi,  _repository.Jobs, _repository.Roles, _repository.Nebulizers, _repository.Tractors, new CommonDbOperations<UserApplicator>());

        public INebulizerOperations Nebulizers => new NebulizerOperations(_repository.Nebulizers, new CommonDbOperations<Nebulizer>());
        
        public ITractorOperations Tractors => new TractorOperations(_repository.Tractors, new CommonDbOperations<Tractor>());

        public ISpecieOperations Species => new SpecieOperations(_repository.Species, new CommonDbOperations<Specie>());

        public IRootstockOperations Rootstock => new RootstockOperations(_repository.Rootstocks, new CommonDbOperations<Rootstock>());

        public IIngredientCategoryOperations IngredientCategories => new IngredientCategoryOperations(_repository.Categories, new CommonDbOperations<IngredientCategory>());

        public IIngredientsOperations Ingredients => new IngredientOperations(_repository.Ingredients, _repository.Categories, new CommonDbOperations<Ingredient>());

        public ISeasonOperations Seasons => new SeasonOperations(_repository.Seasons, new CommonDbOperations<Season>());
        
        public ISectorOperations Sectors => new SectorOperations(_repository.Sectors, new CommonDbOperations<Sector>(), _idSeason);

        public IPlotLandOperations PlotLands => new PlotLandOperations(_repository.PlotLands, _repository.Sectors, new CommonDbOperations<PlotLand>(), _idSeason);

        public IBarrackOperations<Barrack> Barracks => new BarrackOperations<Barrack>(_repository.Barracks, _repository.Rootstocks, _repository.PlotLands, _repository.Varieties, new CommonDbOperations<Barrack>(), _idSeason, _searchServiceInstance);

        public IVarietyOperations Varieties => new VarietyOperations(_repository.Varieties, _repository.Species, new CommonDbOperations<Variety>());

        public IProductOperations<Product> Products => new ProductOperations<Product>(
            _repository.Ingredients,
            _repository.Products,            
            _repository.Targets,
            _repository.CertifiedEntities,
            _repository.Varieties,
            _repository.Species,
            new CommonDbOperations<Product>(),
            _idSeason,
            _searchServiceInstance
        );

        public ICertifiedEntityOperations CertifiedEntities => new CertifiedEntityOperations(_repository.CertifiedEntities, new CommonDbOperations<CertifiedEntity>());

        public ICustomManager CustomManager => new CustomManager(_repository, _idSeason);


        public IApplicationOrderOperations ApplicationOrders => new ApplicationOrderOperations<ApplicationOrder>(new ApplicationOrderArgs {
            ApplicationOrder = _repository.Orders,
            GraphApi = _graphApi,
            Barracks = _repository.Barracks,
            Product = _repository.Products,
            Notifications = _repository.NotificationEvents,
            CommonDb = new ApplicationOrderCommonDbArgs {
                ApplicationOrder = new CommonDbOperations<ApplicationOrder>()
            },
            DosesArgs = new DosesArgs {
                CertifiedEntity = _repository.CertifiedEntities,
                Specie = _repository.Species,
                Target = _repository.Targets,
                Variety = _repository.Varieties
            },
            PreOrder = _repository.PhenologicalPreOrders,
            SeasonId = _idSeason
        }, _searchServiceInstance);

        public INotificatonEventOperations NotificationEvents => new NotificationEventOperations(_repository.NotificationEvents, _repository.Barracks, _repository.PhenologicalEvents, new CommonDbOperations<NotificationEvent>(), _uploadImage, _graphApi, _weatherApi);
        
        public IOrderFolderOperations OrderFolder => new OrderFolderOperations(new OrderFolderArgs {
            Ingredient = _repository.Ingredients,
            IngredientCategory = _repository.Categories,
            OrderFolder = _repository.OrderFolder,
            GraphApi = _graphApi,
            PhenologicalEvent = _repository.PhenologicalEvents,
            Specie = _repository.Species,
            Target = _repository.Targets,
            IdSeason = _idSeason,
            NotificationEvent = _repository.NotificationEvents,
            CommonDb = new CommonDbOperations<OrderFolder>(),
            CommonDbNotifications = new CommonDbOperations<NotificationEvent>()
        });
        public IPhenologicalPreOrderOperations PhenologicalPreOrders => new PhenologicalPreOrdersOperations(_repository.PhenologicalPreOrders, new CommonDbOperations<PhenologicalPreOrder>(), _idSeason, _graphApi);

        public IExecutionOrderOperations<ExecutionOrder> ExecutionOrders => new ExecutionOrderOperations<ExecutionOrder>(_repository.ExecutionOrders, _repository.Orders, _repository.Users, _repository.Nebulizers, _repository.Products, _repository.Tractors, new CommonDbOperations<ExecutionOrder>(), _graphApi, _idSeason, _searchServiceInstance);

        public IBusinessNameOperations BusinessNames => new BusinessNameOperations(_repository.BusinessNames, new CommonDbOperations<BusinessName>(), _graphApi);

        public ICostCenterOperations CostCenters => new CostCenterOperations(_repository.CostCenters, new CommonDbOperations<CostCenter>());
    }
}