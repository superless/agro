using trifenix.agro.db.applicationsReference.agro.events;
using trifenix.agro.db.applicationsReference.agro.fields;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class AgroRepository : IAgroRepository
    {


        private AgroDbArguments _dbArguments;
        public AgroRepository(AgroDbArguments dbArguments)
        {
            _dbArguments = dbArguments;
        }
        public IApplicationTargetRepository Targets => new ApplicationTargetRepository(new MainDb<ApplicationTarget>(_dbArguments));

        public IIngredientCategoryRepository Categories => new IngredientCategoryRepository(new MainDb<IngredientCategory>(_dbArguments));

        public IIngredientRepository Ingredients => new IngredientRepository(new MainDb<Ingredient>(_dbArguments));

        public IPhenologicalEventRepository PhenologicalEvents => new PhenologicalEventRepository(new MainDb<PhenologicalEvent>(_dbArguments));

        public ISpecieRepository Species => new SpecieRepository(new MainDb<Specie>(_dbArguments));

        public ISeasonRepository Seasons => new SeasonRepository(new MainDb<Season>(_dbArguments));

        public IOrderFolderRepository OrderFolder => new OrderFolderRepository(new MainDb<OrderFolder>(_dbArguments));

        public IPhenologicalPreOrderRepository PhenologicalPreOrders => new PhenologicalPreOrderRepository(new MainDb<PhenologicalPreOrder>(_dbArguments));

        public INotificationEventRepository NotificationEvents => new NotificationEventRepository(new MainDb<NotificationEvent>(_dbArguments));

        public IBarrackRepository Barracks => new BarrackRepository(new MainDb<Barrack>(_dbArguments));

        public IPlotLandRepository PlotLands => new PlotLandRepository(new MainDb<PlotLand>(_dbArguments));

        public ISectorRepository Sectors => new SectorRepository(new MainDb<Sector>(_dbArguments));

        public IVarietyRepository Varieties => new VarietyRepository(new MainDb<Variety>(_dbArguments));
    }
}
