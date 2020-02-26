using trifenix.agro.db.applicationsReference.agro.events;
using trifenix.agro.db.applicationsReference.agro.ext;
using trifenix.agro.db.applicationsReference.agro.fields;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.interfaces;
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

        public IMainGenericDb<Sector> Sectors => new SectorRepository(DbArguments);

        public IMainGenericDb<Specie> Species => new SpecieRepository(DbArguments);

        public IMainGenericDb<PlotLand> PlotLands => new PlotLandRepository(DbArguments);


        public IMainGenericDb<Variety> Varieties => new VarietyRepository(DbArguments);


        public IMainGenericDb<ApplicationTarget> Targets => new ApplicationTargetRepository(DbArguments);

        public IMainGenericDb<PhenologicalEvent> PhenologicalEvents => new PhenologicalEventRepository(DbArguments);

        public IMainGenericDb<CertifiedEntity> CertifiedEntities => new CertifiedEntityRepository(DbArguments);


        public IMainGenericDb<IngredientCategory> Categories => new IngredientCategoryRepository(DbArguments);

        public IMainGenericDb<Ingredient> Ingredients => new IngredientRepository(DbArguments);


        public IMainGenericDb<Product> Products => new ProductRepository(DbArguments);

        public IMainGenericDb<Doses> Doses => new DosesRepository(DbArguments);


        public IMainGenericDb<Role> Roles => new RoleRepository(DbArguments);


        public IMainGenericDb<Job> Jobs => new JobRepository(DbArguments);

        

        public IMainGenericDb<UserApplicator> Users => new UserRepository(DbArguments);

        public IMainGenericDb<Nebulizer> Nebulizers => new NebulizerRepository(DbArguments);


        

        public IMainGenericDb<Tractor> Tractors => new TractorRepository(DbArguments);


        public IMainGenericDb<BusinessName> BusinessNames => new BusinessNameRepository(DbArguments);

        public IMainGenericDb<CostCenter> CostCenters => new CostCenterRepository(DbArguments);


        public IMainGenericDb<Season> Seasons => new SeasonRepository(DbArguments);





        public IRootstockRepository Rootstocks => new RootstockRepository(DbArguments);

        


        public IOrderFolderRepository OrderFolder => new OrderFolderRepository(DbArguments);

        public IPhenologicalPreOrderRepository PhenologicalPreOrders => new PhenologicalPreOrderRepository(DbArguments);

        public INotificationEventRepository NotificationEvents => new NotificationEventRepository(DbArguments);

        public IBarrackRepository Barracks => new BarrackRepository(DbArguments);

        

        

        

        

        

        public IApplicationOrderRepository Orders => new ApplicationOrderRepository(DbArguments);

        public IExecutionOrderRepository ExecutionOrders => new ExecutionOrderRepository(DbArguments);

        

        public ICommentRepository Comments => new CommentRepository(DbArguments);

        public IMainDb<UserActivity> UserActivity => new UserActivityRepository(DbArguments);
    }
}