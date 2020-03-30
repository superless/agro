using Cosmonaut;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model;
using trifenix.agro.db.model.core;
using trifenix.agro.db.model.orders;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces
{
    public interface IAgroManager {

        ICosmosStore<EntityContainer> BatchStore { get; }
        IExistElement ExistsElements { get; }
        IGenericOperation<UserActivity, UserActivityInput> UserActivity { get; }
        IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder { get; }
        IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget { get; }
        IGenericOperation<Barrack, BarrackInput> Barrack { get; }
        IGenericOperation<BusinessName, BusinessNameInput> BusinessName { get; }
        IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity { get; }
        IGenericOperation<Comment, CommentInput> Comments { get; }
        IGenericOperation<CostCenter, CostCenterInput> CostCenter { get; }
        IGenericOperation<Dose, DosesInput> Dose { get; }
        IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder { get; }
        IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus { get; }
        IGenericOperation<Ingredient, IngredientInput> Ingredient { get; }
        IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory { get; }
        IGenericOperation<Job, JobInput> Job { get; }
        IGenericOperation<Nebulizer, NebulizerInput> Nebulizer { get; }
        IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent { get; }
        IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder { get; }
        IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent { get; }
        IGenericOperation<PlotLand, PlotLandInput> PlotLand { get; }
        IGenericOperation<PreOrder, PreOrderInput> PreOrder { get; }
        IGenericOperation<Product, ProductInput> Product { get; }
        IGenericOperation<Role, RoleInput> Role { get; }
        IGenericOperation<Rootstock, RootstockInput> Rootstock { get; }
        IGenericOperation<Season, SeasonInput> Season { get; }
        IGenericOperation<Sector, SectorInput> Sector { get; }
        IGenericOperation<Specie, SpecieInput> Specie { get; }
        IGenericOperation<Tractor, TractorInput> Tractor { get; }
        IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator { get; }
        IGenericOperation<Variety, VarietyInput> Variety { get; }

    }

}