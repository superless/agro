using Cosmonaut;
using System;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.external;

namespace trifenix.connect.agro.interfaces.external
{

    public interface IAgroManager<T> {

        IGenericOperation<UserActivity, UserActivityInput> UserActivity { get; }
        IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder { get; }
        IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget { get; }
        IGenericOperation<Barrack, BarrackInput> Barrack { get; }
        IGenericOperation<BusinessName, BusinessNameInput> BusinessName { get; }
        IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity { get; }
        IGenericOperation<Comment, CommentInput> Comments { get; }
        IGenericOperation<CostCenter, CostCenterInput> CostCenter { get; }
        IGenericOperation<Dose, DosesInput> Dose { get; }
        IGenericOperation<Warehouse, WarehouseInput> Warehouse { get; }
        IGenericOperation<WarehouseDocument, WarehouseDocumentInput> WarehouseDocument { get; }
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

        IGenericOperation<Brand, BrandInput> Brand { get; }

        IGenericOperation<Sector, SectorInput> Sector { get; }
        IGenericOperation<Specie, SpecieInput> Specie { get; }
        IGenericOperation<Tractor, TractorInput> Tractor { get; }
        IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator { get; }
        IGenericOperation<Variety, VarietyInput> Variety { get; }

        dynamic GetOperationByInputType(Type InputType);
        dynamic GetOperationByDbType(Type DbType);

    }

}