using trifenix.agro.db.model;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces.entities.core;
using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.interfaces.entities.orders;

namespace trifenix.agro.external.interfaces {
    public interface IAgroManager {
        string IdSeason { get; }
        IApplicationOrderOperations ApplicationOrders { get; }
        IApplicationTargetOperations ApplicationTargets { get; }
        IBarrackOperations<Barrack> Barracks { get; }
        IBusinessNameOperations BusinessNames { get; }
        ICertifiedEntityOperations CertifiedEntities { get; }
        ICostCenterOperations CostCenters { get; }
        ICounterOperations Counter { get; }
        ICustomManager CustomManager { get; }
        IExecutionOrderOperations<ExecutionOrder> ExecutionOrders { get; }
        IIngredientCategoryOperations IngredientCategories { get; }
        IIngredientsOperations Ingredients { get; }
        IJobOperations Jobs { get; }
        INebulizerOperations Nebulizers { get; }
        INotificatonEventOperations NotificationEvents { get; }
        IOrderFolderOperations OrderFolder { get; }
        IPhenologicalOperations PhenologicalEvents { get; }
        IPhenologicalPreOrderOperations PhenologicalPreOrders { get; }
        IPlotLandOperations PlotLands { get; }
        IProductOperations<Product> Products { get; }
        IRoleOperations Roles { get; }
        IRootstockOperations Rootstock { get; }
        ISeasonOperations Seasons { get; }
        ISectorOperations Sectors { get; }
        ISpecieOperations Species { get; }
        ITractorOperations Tractors { get; }
        IUserOperations Users { get; }
        IVarietyOperations Varieties { get; }

    }
}