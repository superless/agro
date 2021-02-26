using Microsoft.Spatial;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.model;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model;
using trifenix.connect.arguments;
using trifenix.connect.mdm.search.model;

namespace trifenix.agro.console {

    class Program {

        static async Task RegenerateIndexFromDB(CommonQueries queriesToDB, AgroSearch<GeographyPoint> searchServiceInstance) {
            var getAll_Query = "SELECT * from c";
            (await queriesToDB.MultipleQuery<ApplicationOrder, ApplicationOrder>(getAll_Query)).ToList().ForEach(ApplicationOrder => searchServiceInstance.AddDocument(ApplicationOrder));
            (await queriesToDB.MultipleQuery<ApplicationTarget, ApplicationTarget>(getAll_Query)).ToList().ForEach(ApplicationTarget => searchServiceInstance.AddDocument(ApplicationTarget));
            (await queriesToDB.MultipleQuery<Barrack, Barrack>(getAll_Query)).ToList().ForEach(Barrack => searchServiceInstance.AddDocument(Barrack));
            (await queriesToDB.MultipleQuery<Brand, Brand>(getAll_Query)).ToList().ForEach(Brand => searchServiceInstance.AddDocument(Brand));
            (await queriesToDB.MultipleQuery<BusinessName, BusinessName>(getAll_Query)).ToList().ForEach(BusinessName => searchServiceInstance.AddDocument(BusinessName));
            (await queriesToDB.MultipleQuery<CertifiedEntity, CertifiedEntity>(getAll_Query)).ToList().ForEach(CertifiedEntity => searchServiceInstance.AddDocument(CertifiedEntity));
            (await queriesToDB.MultipleQuery<Comment, Comment>(getAll_Query)).ToList().ForEach(Comment => searchServiceInstance.AddDocument(Comment));
            (await queriesToDB.MultipleQuery<CostCenter, CostCenter>(getAll_Query)).ToList().ForEach(CostCenter => searchServiceInstance.AddDocument(CostCenter));
            (await queriesToDB.MultipleQuery<Dose, Dose>(getAll_Query)).ToList().ForEach(Dose => searchServiceInstance.AddDocument(Dose));
            (await queriesToDB.MultipleQuery<ExecutionOrder, ExecutionOrder>(getAll_Query)).ToList().ForEach(ExecutionOrder => searchServiceInstance.AddDocument(ExecutionOrder));
            (await queriesToDB.MultipleQuery<ExecutionOrderStatus, ExecutionOrderStatus>(getAll_Query)).ToList().ForEach(ExecutionOrderStatus => searchServiceInstance.AddDocument(ExecutionOrderStatus));
            (await queriesToDB.MultipleQuery<Ingredient, Ingredient>(getAll_Query)).ToList().ForEach(Ingredient => searchServiceInstance.AddDocument(Ingredient));
            (await queriesToDB.MultipleQuery<IngredientCategory, IngredientCategory>(getAll_Query)).ToList().ForEach(IngredientCategory => searchServiceInstance.AddDocument(IngredientCategory));
            (await queriesToDB.MultipleQuery<Job, Job>(getAll_Query)).ToList().ForEach(Job => searchServiceInstance.AddDocument(Job));
            (await queriesToDB.MultipleQuery<Nebulizer, Nebulizer>(getAll_Query)).ToList().ForEach(Nebulizer => searchServiceInstance.AddDocument(Nebulizer));
            (await queriesToDB.MultipleQuery<NotificationEvent, NotificationEvent>(getAll_Query)).ToList().ForEach(NotificationEvent => searchServiceInstance.AddDocument(NotificationEvent));
            (await queriesToDB.MultipleQuery<OrderFolder, OrderFolder>(getAll_Query)).ToList().ForEach(OrderFolder => searchServiceInstance.AddDocument(OrderFolder));
            (await queriesToDB.MultipleQuery<PhenologicalEvent, PhenologicalEvent>(getAll_Query)).ToList().ForEach(PhenologicalEvent => searchServiceInstance.AddDocument(PhenologicalEvent));
            (await queriesToDB.MultipleQuery<PlotLand, PlotLand>(getAll_Query)).ToList().ForEach(PlotLand => searchServiceInstance.AddDocument(PlotLand));
            (await queriesToDB.MultipleQuery<PreOrder, PreOrder>(getAll_Query)).ToList().ForEach(PreOrder => searchServiceInstance.AddDocument(PreOrder));
            (await queriesToDB.MultipleQuery<Product, Product>(getAll_Query)).ToList().ForEach(Product => searchServiceInstance.AddDocument(Product));
            (await queriesToDB.MultipleQuery<OrderFolder, OrderFolder>(getAll_Query)).ToList().ForEach(OrderFolder => searchServiceInstance.AddDocument(OrderFolder));
            (await queriesToDB.MultipleQuery<Role, Role>(getAll_Query)).ToList().ForEach(Role => searchServiceInstance.AddDocument(Role));
            (await queriesToDB.MultipleQuery<Rootstock, Rootstock>(getAll_Query)).ToList().ForEach(Rootstock => searchServiceInstance.AddDocument(Rootstock));
            (await queriesToDB.MultipleQuery<Season, Season>(getAll_Query)).ToList().ForEach(Season => searchServiceInstance.AddDocument(Season));
            (await queriesToDB.MultipleQuery<Sector, Sector>(getAll_Query)).ToList().ForEach(Sector => searchServiceInstance.AddDocument(Sector));
            (await queriesToDB.MultipleQuery<Specie, Specie>(getAll_Query)).ToList().ForEach(Specie => searchServiceInstance.AddDocument(Specie));
            (await queriesToDB.MultipleQuery<Tractor, Tractor>(getAll_Query)).ToList().ForEach(Tractor => searchServiceInstance.AddDocument(Tractor));
            (await queriesToDB.MultipleQuery<Rootstock, Rootstock>(getAll_Query)).ToList().ForEach(Rootstock => searchServiceInstance.AddDocument(Rootstock));
            (await queriesToDB.MultipleQuery<UserApplicator, UserApplicator>(getAll_Query)).ToList().ForEach(UserApplicator => searchServiceInstance.AddDocument(UserApplicator));
            (await queriesToDB.MultipleQuery<Variety, Variety>(getAll_Query)).ToList().ForEach(Variety => searchServiceInstance.AddDocument(Variety));
            (await queriesToDB.MultipleQuery<Warehouse, Warehouse>(getAll_Query)).ToList().ForEach(Warehouse => searchServiceInstance.AddDocument(Warehouse));
            (await queriesToDB.MultipleQuery<WarehouseDocument, WarehouseDocument>(getAll_Query)).ToList().ForEach(Warehouse => searchServiceInstance.AddDocument(Warehouse));
        }

        static async Task Main(string[] args) {
            var queriesToDB = new CommonQueries(new CosmosDbArguments { EndPointUrl = "https://agro-cosmodb.documents.azure.com:443/" , NameDb = "agro-cosmodb", PrimaryKey = "kaPYpzhFCcG1bk3aC69aX1T2amavVi8TfHmrIMNJuhpYXtIz67PMhwBKctunNzclFBcxypZvcjPUW846YZuvjA==" });
            var searchServiceInstance = new AgroSearch<GeographyPoint>("https://fenix-search.search.windows.net/", "EFF07EE3D5A0C74C2363EC4DDB9710D7", new ImplementsSearch(), new HashEntityAgroSearch());
            await RegenerateIndexFromDB(queriesToDB, searchServiceInstance);
        }

    }

}