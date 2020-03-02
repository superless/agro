using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces {
    public interface IAgroManager {

        IGenericOperation<Sector, SectorInput> Sectors { get; }


        IGenericFullReadOperation<UserActivity, UserActivityInput> UserActivity { get; }


        IGenericOperation<PlotLand, PlotLandInput> PlotLands { get; }


        IGenericOperation<Specie, SpecieInput> Species { get; }

        IGenericOperation<Variety, VarietyInput> Varieties { get; }


        IGenericOperation<ApplicationTarget, TargetInput> ApplicationTargets { get; }

        IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvents { get; }

        IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntities { get; }


        IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategories { get; }

        IGenericOperation<Ingredient, IngredientInput> Ingredients { get; }

        IGenericOperation<Product, ProductInput> Products { get; }

        IGenericOperation<Doses, DosesInput> Doses { get; }

        IGenericOperation<Job, JobInput> Jobs { get; }
        IGenericOperation<Role, RoleInput> Roles { get; }

        IGenericOperation<UserApplicator, UserApplicatorInput> Users { get; }

        IGenericOperation<Nebulizer, NebulizerInput> Nebulizers { get; }

        IGenericOperation<Tractor, TractorInput> Tractors { get; }


        IGenericOperation<BusinessName, BusinessNameInput> BusinessNames { get; }

        IGenericOperation<CostCenter, CostCenterInput> CostCenters { get; }


        IGenericOperation<Season, SeasonInput> Seasons { get; }
        IGenericOperation<Rootstock, RootStockInput> Rootstock { get; }

        IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder { get; }

        IGenericOperation<Barrack, BarrackInput> Barracks { get; }

        IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvents { get; }


        IGenericOperation<PreOrder, PreOrderInput> PreOrders { get; }

        IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrders { get; }

        IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionStatus { get; }















        IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrders { get; }

         


        IGraphApi GraphApi { get; }

        IExistElement ExistsElements { get; }
    }
}
