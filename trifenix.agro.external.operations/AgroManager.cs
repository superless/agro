using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.common;
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
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations {
    public class AgroManager : IAgroManager {

        private readonly IAgroRepository _repository;
        
        private readonly IUploadImage _uploadImage;
        private readonly IGraphApi _graphApi;
        private readonly IWeatherApi _weatherApi;
        private readonly IAgroSearch _searchServiceInstance;

        public AgroManager(IAgroRepository repository,  IUploadImage uploadImage, IGraphApi graphApi, IWeatherApi weatherApi, IAgroSearch searchServiceInstance) {
            _repository = repository;            
            _uploadImage = uploadImage;
            
            _weatherApi = weatherApi;
            _searchServiceInstance = searchServiceInstance;
            GraphApi = graphApi;

        }

        public IGraphApi GraphApi { get; }

        public IGenericFullReadOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations(_repository.UserActivity, new CommonDbOperations<UserActivity>(), GraphApi);

        public IExistElement ExistsElements => new CosmosExistElement(_repository.DbArguments);

        public IGenericOperation<Sector, SectorInput> Sectors => new SectorOperations(_repository.Sectors, ExistsElements, _searchServiceInstance);
        public IGenericOperation<PlotLand, PlotLandInput> PlotLands => new PlotLandOperations(_repository.PlotLands, ExistsElements, _searchServiceInstance);


        public IGenericOperation<Specie, SpecieInput> Species => new SpecieOperations(_repository.Species, ExistsElements, _searchServiceInstance);

        public IGenericOperation<Variety, VarietyInput> Varieties => new VarietyOperations(_repository.Varieties, ExistsElements, _searchServiceInstance);


        public IGenericOperation<ApplicationTarget, TargetInput> ApplicationTargets => new ApplicationTargetOperations(_repository.Targets, ExistsElements, _searchServiceInstance);



        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvents => new PhenologicalEventOperations(_repository.PhenologicalEvents, ExistsElements, _searchServiceInstance);


        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntities => new CertifiedEntityOperations(_repository.CertifiedEntities, ExistsElements, _searchServiceInstance);

        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategories => new IngredientCategoryOperations(_repository.Categories, ExistsElements, _searchServiceInstance);

        public IGenericOperation<Ingredient, IngredientInput> Ingredients => new IngredientOperations(_repository.Ingredients, ExistsElements, _searchServiceInstance);


        public IGenericOperation<Product, ProductInput> Products => new ProductOperations(_repository.Products, ExistsElements, _searchServiceInstance, Doses);


        public IGenericOperation<Doses, DosesInput> Doses => new DosesOperations(_repository.Doses, ExistsElements, _searchServiceInstance);

        public IGenericOperation<Role, RoleInput> Roles => new RoleOperations(_repository.Roles, _searchServiceInstance);



        public IGenericOperation<Job, JobInput> Jobs => new JobOperations(_repository.Jobs, ExistsElements, _searchServiceInstance);

        

        public IGenericOperation<UserApplicator, UserApplicatorInput> Users => new UserOperations(_repository.Users, ExistsElements, _searchServiceInstance, 
            GraphApi);


        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizers => new NebulizerOperations(_repository.Nebulizers, ExistsElements, _searchServiceInstance);
        


        public IGenericOperation<Tractor, TractorInput> Tractors => new TractorOperations(_repository.Tractors, ExistsElements, _searchServiceInstance);



        public IGenericOperation<BusinessName, BusinessNameInput> BusinessNames => new BusinessNameOperations(_repository.BusinessNames, ExistsElements, _searchServiceInstance);

        public IGenericOperation<CostCenter, CostCenterInput> CostCenters => new CostCenterOperations(_repository.CostCenters, ExistsElements, _searchServiceInstance);

        public IRootstockOperations Rootstock => new RootstockOperations(_repository.Rootstocks, new CommonDbOperations<Rootstock>());

        

        

        public ISeasonOperations Seasons => new SeasonOperations(_repository.Seasons, new CommonDbOperations<Season>());
        
       

        public IBarrackOperations<Barrack> Barracks => new BarrackOperations<Barrack>(_repository.Barracks, _repository.Rootstocks, _repository.PlotLands, _repository.Varieties, new CommonDbOperations<Barrack>(), _idSeason, _searchServiceInstance);

        
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

        //public IExecutionOrderOperations<ExecutionOrder> ExecutionOrders => new ExecutionOrderOperations<ExecutionOrder>(_repository.ExecutionOrders, _repository.Orders, _repository.Users, _repository.Nebulizers, _repository.Products, _repository.Tractors, new CommonDbOperations<ExecutionOrder>(), _graphApi, _idSeason, _searchServiceInstance);

        

        

        
    }
}