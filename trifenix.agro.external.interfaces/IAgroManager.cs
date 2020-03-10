using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces
{
    public interface IAgroManager {

        IExistElement ExistsElements { get; }
        IGenericFullReadOperation<UserActivity, UserActivityInput> UserActivity { get; }
        IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrders { get; }
        IGenericOperation<ApplicationTarget, TargetInput> ApplicationTargets { get; }
        IGenericOperation<Barrack, BarrackInput> Barracks { get; }
        IGenericOperation<BusinessName, BusinessNameInput> BusinessNames { get; }
        IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntities { get; }
        IGenericOperation<CostCenter, CostCenterInput> CostCenters { get; }
        IGenericOperation<Doses, DosesInput> Doses { get; }
        IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrders { get; }
        IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionStatus { get; }
        IGenericOperation<Ingredient, IngredientInput> Ingredients { get; }
        IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategories { get; }
        IGenericOperation<Job, JobInput> Jobs { get; }
        IGenericOperation<Nebulizer, NebulizerInput> Nebulizers { get; }
        IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvents { get; }
        IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder { get; }
        IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvents { get; }
        IGenericOperation<PlotLand, PlotLandInput> PlotLands { get; }
        IGenericOperation<PreOrder, PreOrderInput> PreOrders { get; }
        IGenericOperation<Product, ProductInput> Products { get; }
        IGenericOperation<Role, RoleInput> Roles { get; }
        IGenericOperation<Rootstock, RootStockInput> Rootstock { get; }
        IGenericOperation<Season, SeasonInput> Seasons { get; }
        IGenericOperation<Sector, SectorInput> Sectors { get; }
        IGenericOperation<Specie, SpecieInput> Species { get; }
        IGenericOperation<Tractor, TractorInput> Tractors { get; }
        IGenericOperation<UserApplicator, UserApplicatorInput> Users { get; }
        IGenericOperation<Variety, VarietyInput> Varieties { get; }

    }
}