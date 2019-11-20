using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.interfaces.entities.orders;

namespace trifenix.agro.external.interfaces
{
    public interface IAgroManager
    {
        IPhenologicalOperations PhenologicalEvents { get; }

        IApplicationTargetOperations ApplicationTargets { get; }

        ISpecieOperations Species { get; }

        IRootstockOperations Rootstock { get; }

        IIngredientCategoryOperations IngredientCategories { get; }

        IIngredientsOperations Ingredients { get; }

        ISeasonOperations Seasons { get; }

        IOrderFolderOperations OrderFolder { get; }

        ISectorOperations Sectors { get; }

        IPlotLandOperations PlotLands { get; }

        IBarrackOperations Barracks { get; }

        IPhenologicalPreOrderOperations PhenologicalPreOrders { get; }

        INotificatonEventOperations NotificationEvents { get; }

        IVarietyOperations Varieties { get; }

        IProductOperations Products { get; }

        ICertifiedEntityOperations CertifiedEntities { get; }

        ICustomManager CustomManager { get; }






    }
}
