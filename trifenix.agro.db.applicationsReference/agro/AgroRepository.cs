using trifenix.agro.db.applicationsReference.agro.events;
using trifenix.agro.db.applicationsReference.agro.ext;
using trifenix.agro.db.applicationsReference.agro.fields;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class AgroRepository : IAgroRepository
    {
        public AgroDbArguments DbArguments { get; }
        public AgroRepository(AgroDbArguments dbArguments)
        {
            DbArguments = dbArguments;
        }

        public IApplicationTargetRepository Targets => new ApplicationTargetRepository(new MainDb<ApplicationTarget>(DbArguments));

        public IJobRepository Jobs => new JobRepository(new MainDb<Job>(DbArguments));

        public IRoleRepository Roles => new RoleRepository(new MainDb<Role>(DbArguments));

        public IUserRepository Users => new UserRepository(new MainDb<UserApplicator>(DbArguments));

        public INebulizerRepository Nebulizers => new NebulizerRepository(new MainDb<Nebulizer>(DbArguments));

        public ITractorRepository Tractors => new TractorRepository(new MainDb<Tractor>(DbArguments));

        public IIngredientCategoryRepository Categories => new IngredientCategoryRepository(new MainDb<IngredientCategory>(DbArguments));

        public IIngredientRepository Ingredients => new IngredientRepository(new MainDb<Ingredient>(DbArguments));

        public IPhenologicalEventRepository PhenologicalEvents => new PhenologicalEventRepository(new MainDb<PhenologicalEvent>(DbArguments));

        public ISpecieRepository Species => new SpecieRepository(new MainDb<Specie>(DbArguments));

        public IRootstockRepository Rootstocks => new RootstockRepository(new MainDb<Rootstock>(DbArguments));

        public ISeasonRepository Seasons => new SeasonRepository(new MainDb<Season>(DbArguments));

        public IOrderFolderRepository OrderFolder => new OrderFolderRepository(new MainDb<OrderFolder>(DbArguments));

        public IPhenologicalPreOrderRepository PhenologicalPreOrders => new PhenologicalPreOrderRepository(new MainDb<PhenologicalPreOrder>(DbArguments));

        public INotificationEventRepository NotificationEvents => new NotificationEventRepository(new MainDb<NotificationEvent>(DbArguments));

        public IBarrackRepository Barracks => new BarrackRepository(new MainDb<Barrack>(DbArguments));

        public IPlotLandRepository PlotLands => new PlotLandRepository(new MainDb<PlotLand>(DbArguments));

        public ISectorRepository Sectors => new SectorRepository(new MainDb<Sector>(DbArguments));

        public IVarietyRepository Varieties => new VarietyRepository(new MainDb<Variety>(DbArguments));

        public IProductRepository Products => new ProductRepository(new MainDb<Product>(DbArguments));

        public ICertifiedEntityRepository CertifiedEntities => new CertifiedEntityRepository(new MainDb<CertifiedEntity>(DbArguments));

        public IApplicationOrderRepository Orders => new ApplicationOrderRepository(new MainDb<ApplicationOrder>(DbArguments));

        public IExecutionOrderRepository ExecutionOrders => new ExecutionOrderRepository(new MainDb<ExecutionOrder>(DbArguments));

        public IBusinessNameRepository BusinessNames => new BusinessNameRepository(new MainDb<BusinessName>(DbArguments));

        public ICostCenterRepository CostCenters => new CostCenterRepository(new MainDb<CostCenter>(DbArguments));

    }
}