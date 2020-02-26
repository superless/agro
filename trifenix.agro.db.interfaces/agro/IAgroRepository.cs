using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IAgroRepository
    {
        AgroDbArguments DbArguments { get; }

        IMainGenericDb<PlotLand> PlotLands { get; }

        IMainGenericDb<Sector> Sectors { get; }

        IMainGenericDb<Specie> Species { get; }

        IMainGenericDb<Variety> Varieties { get; }
        IMainGenericDb<ApplicationTarget> Targets { get; }

        IMainGenericDb<PhenologicalEvent> PhenologicalEvents { get; }


        IMainGenericDb<CertifiedEntity> CertifiedEntities { get; }

        IMainGenericDb<IngredientCategory> Categories { get; }

        IMainGenericDb<Ingredient> Ingredients { get; }

        IMainGenericDb<Product> Products { get; }


        IMainGenericDb<Doses> Doses { get; }

        IMainGenericDb<Role> Roles { get; }

        IMainGenericDb<Job> Jobs { get; }



        IMainGenericDb<UserApplicator> Users { get; }

        IMainGenericDb<Nebulizer> Nebulizers { get; }

        IMainGenericDb<Tractor> Tractors { get; }

        IMainGenericDb<BusinessName> BusinessNames { get; }

        IMainGenericDb<CostCenter> CostCenters { get; }

        IMainGenericDb<Season> Seasons { get; }






        IRootstockRepository Rootstocks { get; }

        


        

        IOrderFolderRepository OrderFolder { get; }
        
        IPhenologicalPreOrderRepository PhenologicalPreOrders { get; }

        INotificationEventRepository NotificationEvents { get; }

        IBarrackRepository Barracks { get; }

        


        

        


        IApplicationOrderRepository Orders { get;  }

        IExecutionOrderRepository ExecutionOrders { get; }

       

        ICommentRepository Comments { get; }




        IMainDb<UserActivity> UserActivity { get; }

        


    }
}
