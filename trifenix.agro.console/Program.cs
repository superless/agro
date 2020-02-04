using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.search;
using trifenix.agro.search.model;

namespace trifenix.agro.console {
    class Program {
        static async Task Main(string[] args){

            //var db = new AgroRepository(new AgroDbArguments {
            //    EndPointUrl = "https://agricola-db.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="
            //});



            //var orders = db.Orders.GetApplicationOrders();
            //orders.ToList().ForEach(order => {
            //    order.isPhenological = order.PhenologicalPreOrders.Count() != 0;
            //    db.Orders.CreateUpdate(order);
            //});



            //var searchServiceInstance = new AgroSearch("agrisearch","F9189208F49AF7C3DFD34E45A89F19E4","entities");
            //var db = new AgroRepository(new AgroDbArguments{
            //    EndPointUrl = "https://agricola-db.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="
            //});

            //var productsQuery = db.Products.GetProducts();
            //var dbProductOperations = new CommonDbOperations<Product>();
            //List<Product> products = await dbProductOperations.TolistAsync(productsQuery);

            //var ordersQuery = db.Orders.GetApplicationOrders();
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);

            //var executionsQuery = db.ExecutionOrders.GetExecutionOrders();
            //var dbExecutionsOperations = new CommonDbOperations<ExecutionOrder>();
            //List<ExecutionOrder> executions = await dbExecutionsOperations.TolistAsync(executionsQuery);

            //var productsIndex = products.Select(product => new EntitySearch { Created = DateTime.Now, EntityName = "Product", Id = product.Id, Name = product.CommercialName }).ToList();
            //var ordersIndex = orders.Select(order => new EntitySearch { Created = DateTime.Now, EntityName = "ApplicationOrder", Id = order.Id, Name = order.Name }).ToList();
            //var executionsIndex = executions.Select(execution => new EntitySearch { Created = DateTime.Now, EntityName = "ExecutionOrder", Id = execution.Id, Name = execution.Name }).ToList();

            //searchServiceInstance.AddEntities(productsIndex);
            //searchServiceInstance.AddEntities(ordersIndex);
            //searchServiceInstance.AddEntities(executionsIndex);

            //CosmosStore<Barrack> store = new CosmosStore<Barrack>(new CosmosStoreSettings("agrodb", "https://agricola-db.documents.azure.com:443/", "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="));
            //await store.RemoveAsync(barrack => !"CIRUELA DURAZNOS KIWIS NECTARINES CEREZAS MANZANA".Split().Contains(barrack.Variety.Specie.Name));
        }
    }
}
