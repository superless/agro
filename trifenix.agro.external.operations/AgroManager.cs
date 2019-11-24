using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces;
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
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations
{
    public class AgroManager : IAgroManager
    {

        private readonly IAgroRepository _repository;
        private readonly string _idSeason;
        private readonly IUploadImage _uploadImage;

        public AgroManager(IAgroRepository repository, string idSeason, IUploadImage uploadImage)
        {
            _repository = repository;
            _idSeason = idSeason;
            _uploadImage = uploadImage;
        }

        public IPhenologicalOperations PhenologicalEvents => new PhenologicalEventOperations(_repository.PhenologicalEvents, new CommonDbOperations<PhenologicalEvent>());

        public IApplicationTargetOperations ApplicationTargets => new ApplicationTargetOperations(_repository.Targets, new CommonDbOperations<ApplicationTarget>());

        public ISpecieOperations Species => new SpecieOperations(_repository.Species, new CommonDbOperations<Specie>());

        public IRootstockOperations Rootstock => new RootstockOperations(_repository.Rootstock, new CommonDbOperations<Rootstock>());

        public IIngredientCategoryOperations IngredientCategories => new IngredientCategoryOperations(_repository.Categories, new CommonDbOperations<IngredientCategory>());

        public IIngredientsOperations Ingredients => new IngredientOperations(_repository.Ingredients, _repository.Categories, new CommonDbOperations<Ingredient>());

        public ISeasonOperations Seasons => new SeasonOperations(_repository.Seasons, new CommonDbOperations<Season>());

        public IOrderFolderOperations OrderFolder => new OrderFolderOperations(new OrderFolderArgs {
            Ingredient = _repository.Ingredients,
            IngredientCategory = _repository.Categories,
            OrderFolder = _repository.OrderFolder,
            PhenologicalEvent = _repository.PhenologicalEvents,
            Specie = _repository.Species,
            Target = _repository.Targets,
            IdSeason = _idSeason,
            NotificationEvent = _repository.NotificationEvents,
            CommonDb= new CommonDbOperations<OrderFolder>(),
            CommonDbNotifications = new CommonDbOperations<NotificationEvent>()
        });

        public ISectorOperations Sectors => new SectorOperations(_repository.Sectors, new CommonDbOperations<Sector>(), _idSeason);

        public IPlotLandOperations PlotLands => new PlotLandOperations(_repository.PlotLands, _repository.Sectors, new CommonDbOperations<PlotLand>(), _idSeason);

        public IBarrackOperations Barracks => new BarrackOperations(_repository.Barracks, _repository.Rootstock, _repository.PlotLands, _repository.Varieties, new CommonDbOperations<Barrack>(), _idSeason);

        public IPhenologicalPreOrderOperations PhenologicalPreOrders => new PhenologicalPreOrdersOperations(_repository.PhenologicalPreOrders, new CommonDbOperations<PhenologicalPreOrder>(), _idSeason);

        public INotificatonEventOperations NotificationEvents => new NotificationEventOperations(_repository.NotificationEvents, _repository.Barracks, _repository.PhenologicalEvents, new CommonDbOperations<NotificationEvent>(), _uploadImage);

        public IVarietyOperations Varieties => new VarietyOperations(_repository.Varieties, _repository.Species, new CommonDbOperations<Variety>());

        public IProductOperations Products => new ProductOperations(_repository.Ingredients,
            _repository.Products,            
            _repository.Targets,
            _repository.CertifiedEntities,
            _repository.Varieties,
            _repository.Species,
            new CommonDbOperations<Product>(),
            _idSeason
            );

        public ICertifiedEntityOperations CertifiedEntities => new CertifiedEntityOperations(_repository.CertifiedEntities, new CommonDbOperations<CertifiedEntity>());

        public ICustomManager CustomManager => new CustomManager(_repository, _idSeason);


        public IApplicationOrderOperations ApplicationOrders => new ApplicationOrderOperations(new ApplicationOrderArgs { 
            ApplicationOrder = _repository.Order,
            Barracks = _repository.Barracks,
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
        });

    }
}
