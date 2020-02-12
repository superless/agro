using trifenix.agro.db.model.agro;
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
        IPhenologicalOperations PhenologicalEvents { get; }

        IApplicationTargetOperations ApplicationTargets { get; }

        IJobOperations Jobs { get; }

        IRoleOperations Roles { get; }

        IUserOperations Users { get; }

        ITractorOperations Tractors { get; }

        INebulizerOperations Nebulizers { get; }

        ISpecieOperations Species { get; }

        IRootstockOperations Rootstock { get; }

        IIngredientCategoryOperations IngredientCategories { get; }

        IIngredientsOperations Ingredients { get; }

        ISeasonOperations Seasons { get; }

        IOrderFolderOperations OrderFolder { get; }

        ISectorOperations Sectors { get; }

        IPlotLandOperations PlotLands { get; }

        IBarrackOperations<Barrack> Barracks { get; }

        IPhenologicalPreOrderOperations PhenologicalPreOrders { get; }

        INotificatonEventOperations NotificationEvents { get; }

        IVarietyOperations Varieties { get; }

        IProductOperations<Product> Products { get; }

        ICertifiedEntityOperations CertifiedEntities { get; }

        ICustomManager CustomManager { get; }

        IApplicationOrderOperations ApplicationOrders { get; }

        IExecutionOrderOperations<ExecutionOrder> ExecutionOrders { get; }

         IBusinessNameOperations BusinessNames { get; }

        ICostCenterOperations CostCenters { get; }

    }
}
