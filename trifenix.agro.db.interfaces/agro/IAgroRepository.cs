using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;

namespace trifenix.agro.db.interfaces.agro {
    public interface IAgroRepository {
        AgroDbArguments DbArguments { get; }
        IApplicationOrderRepository Orders { get; }
        IApplicationTargetRepository Targets { get; }
        IBarrackRepository Barracks { get; }
        IBusinessNameRepository BusinessNames { get; }
        ICertifiedEntityRepository CertifiedEntities { get; }
        ICostCenterRepository CostCenters { get; }
        ICounterRepository Counter { get; }
        IExecutionOrderRepository ExecutionOrders { get; }
        IIngredientCategoryRepository Categories { get; }
        IIngredientRepository Ingredients { get; }
        IJobRepository Jobs { get; }
        INebulizerRepository Nebulizers { get; }
        INotificationEventRepository NotificationEvents { get; }
        IOrderFolderRepository OrderFolder { get; }
        IPhenologicalEventRepository PhenologicalEvents { get; }
        IPhenologicalPreOrderRepository PhenologicalPreOrders { get; }
        IPlotLandRepository PlotLands { get; }
        IProductRepository Products { get; }
        IRoleRepository Roles { get; }
        IRootstockRepository Rootstocks { get; }
        ISeasonRepository Seasons { get; }
        ISectorRepository Sectors { get; }
        ISpecieRepository Species { get; }
        ITractorRepository Tractors { get; }
        IUserRepository Users { get; }
        IVarietyRepository Varieties { get; }

    }
}