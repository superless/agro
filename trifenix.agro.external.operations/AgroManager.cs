using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro.common;
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
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations
{
    public class AgroManager : IAgroManager {

        
        private readonly AgroDbArguments arguments;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IGraphApi _graphApi;
        private readonly IWeatherApi _weatherApi;
        private readonly IAgroSearch _searchServiceInstance;

        public AgroManager(AgroDbArguments arguments, IEmail email, IUploadImage uploadImage, IGraphApi graphApi, IWeatherApi weatherApi, IAgroSearch searchServiceInstance) {
            this.arguments = arguments;
            _email = email;
            _uploadImage = uploadImage;
            _weatherApi = weatherApi;
            _searchServiceInstance = searchServiceInstance;
            GraphApi = graphApi;

        }

        public IGraphApi GraphApi { get; }

        public IGenericFullReadOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations(new MainDb<UserActivity>(arguments), new CommonDbOperations<UserActivity>(), GraphApi);

        public IExistElement ExistsElements => new CosmosExistElement(arguments);


        public ICommonQueries CommonQueries => new CommonQueries(arguments);


        public IGenericOperation<Sector, SectorInput> Sectors => new SectorOperations(new MainGenericDb<Sector>(arguments), ExistsElements, _searchServiceInstance);
        public IGenericOperation<PlotLand, PlotLandInput> PlotLands => new PlotLandOperations(new MainGenericDb<PlotLand>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<Specie, SpecieInput> Species => new SpecieOperations(new MainGenericDb<Specie>(arguments), ExistsElements, _searchServiceInstance);

        public IGenericOperation<Variety, VarietyInput> Varieties => new VarietyOperations(new MainGenericDb<Variety>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<ApplicationTarget, TargetInput> ApplicationTargets => new ApplicationTargetOperations(new MainGenericDb<ApplicationTarget>(arguments), ExistsElements, _searchServiceInstance);



        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvents => new PhenologicalEventOperations(new MainGenericDb<PhenologicalEvent>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntities => new CertifiedEntityOperations(new MainGenericDb<CertifiedEntity>(arguments), ExistsElements, _searchServiceInstance);

        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategories => new IngredientCategoryOperations(new MainGenericDb<IngredientCategory>(arguments), ExistsElements, _searchServiceInstance);

        public IGenericOperation<Ingredient, IngredientInput> Ingredients => new IngredientOperations(new MainGenericDb<Ingredient>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<Product, ProductInput> Products => new ProductOperations(new MainGenericDb<Product>(arguments), ExistsElements, _searchServiceInstance, Doses);


        public IGenericOperation<Doses, DosesInput> Doses => new DosesOperations(new MainGenericDb<Doses>(arguments), ExistsElements, _searchServiceInstance);

        public IGenericOperation<Role, RoleInput> Roles => new RoleOperations(new MainGenericDb<Role>(arguments), ExistsElements, _searchServiceInstance);



        public IGenericOperation<Job, JobInput> Jobs => new JobOperations(new MainGenericDb<Job>(arguments), ExistsElements, _searchServiceInstance);

        

        public IGenericOperation<UserApplicator, UserApplicatorInput> Users => new UserOperations(new MainGenericDb<UserApplicator>(arguments), ExistsElements, _searchServiceInstance, 
            GraphApi);


        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizers => new NebulizerOperations(new MainGenericDb<Nebulizer>(arguments), ExistsElements, _searchServiceInstance);
        


        public IGenericOperation<Tractor, TractorInput> Tractors => new TractorOperations(new MainGenericDb<Tractor>(arguments), ExistsElements, _searchServiceInstance);



        public IGenericOperation<BusinessName, BusinessNameInput> BusinessNames => new BusinessNameOperations(new MainGenericDb<BusinessName>(arguments), ExistsElements, _searchServiceInstance);

        public IGenericOperation<CostCenter, CostCenterInput> CostCenters => new CostCenterOperations(new MainGenericDb<CostCenter>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<Season, SeasonInput> Seasons => new SeasonOperations(new MainGenericDb<Season>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<Rootstock, RootStockInput> Rootstock => new RootstockOperations(new MainGenericDb<Rootstock>(arguments), ExistsElements, _searchServiceInstance);


        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations(new MainGenericDb<OrderFolder>(arguments), ExistsElements, _searchServiceInstance, CommonQueries);


        public IGenericOperation<Barrack, BarrackInput> Barracks => new BarrackOperations(new MainGenericDb<Barrack>(arguments), ExistsElements, _searchServiceInstance, CommonQueries);



        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvents => new NotificationEventOperations(new MainGenericDb<NotificationEvent>(arguments), ExistsElements, _searchServiceInstance, CommonQueries, _email, _uploadImage, _weatherApi);


        public IGenericOperation<PreOrder, PreOrderInput> PreOrders => new PreOrdersOperations(new MainGenericDb<PreOrder>(arguments), ExistsElements, _searchServiceInstance, CommonQueries);



        


        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrders => new ApplicationOrderOperations(new MainGenericDb<ApplicationOrder>(arguments), ExistsElements, _searchServiceInstance, CommonQueries);




        

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrders => new ExecutionOrderOperations(new MainGenericDb<ExecutionOrder>(arguments), ExistsElements, _searchServiceInstance, CommonQueries);

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionStatus => new ExecutionOrderStatusOperations(new MainGenericDb<ExecutionOrderStatus>(arguments), ExistsElements, _searchServiceInstance);


    }
}