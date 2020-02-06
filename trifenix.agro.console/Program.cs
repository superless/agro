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

            //var db = new AgroRepository(new AgroDbArguments
            //{
            //    EndPointUrl = "https://agricola-db.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="
            //});

            //var query = db.Orders.GetApplicationOrders();
            //var errorList = new List<string>();
            //int indice = 0;
            //int porcentaje = 0;
            //int cantidad = query.Count();
            //int percentilCada = cantidad / 100;
            //Console.WriteLine("Total: " + cantidad);
            //query.ToList().ForEach(order => {
            //    if (indice++ % percentilCada == 0)
            //        Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    if (order.Barracks.Any(barrackInstance => db.Barracks.GetBarrack(barrackInstance.Barrack.Id).Result == null) || order.IdsSpecies.Any(idSpecie => db.Species.GetSpecie(idSpecie).Result == null) || order.IdVarieties.Any(idVariety => db.Varieties.GetVariety(idVariety).Result == null))
            //        errorList.Add(order.Id);
            //});
            //Console.WriteLine("Cantidad con error: " + errorList.Count());
            ////CosmosStore<PhenologicalPreOrder> store = new CosmosStore<PhenologicalPreOrder>(new CosmosStoreSettings("agrodb", "https://agricola-db.documents.azure.com:443/", "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="));
            //errorList.ToList().ForEach(idOrder => {
            //    Console.WriteLine(idOrder);
            //});

            //var ordersQuery = db.Orders.GetApplicationOrders().Where(order => order.PhenologicalPreOrders.Any(ph => ph.Id.Equals("79d69acc62a8488f914bab9d1333a80e")));
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);


            //var orderError = new List<ApplicationOrder> { db.Orders.GetApplicationOrder("60daedf5312e4dbbbd9e21d55a131adc").Result, db.Orders.GetApplicationOrder("a0e6836a75954b36a103172ab9be0f39").Result };
            //orderError.ForEach(order => {
            //    Console.WriteLine("Orden: " + order.Id);
            //    if (order.Barracks.Any(barrackInstance => db.Barracks.GetBarrack(barrackInstance.Barrack.Id).Result == null))
            //        Console.WriteLine("Error en barrack");
            //    if(order.IdsSpecies.Any(idSpecie => db.Species.GetSpecie(idSpecie).Result == null))
            //        Console.WriteLine("Error en especie");
            //    if (order.IdVarieties.Any(idVariety => db.Varieties.GetVariety(idVariety).Result == null))
            //        Console.WriteLine("Error en variedad\n");
            //});


            //var orders = db.Orders.GetApplicationOrders();
            //orders.ToList().ForEach(order => {
            //    order.isPhenological = order.PhenologicalPreOrders.Count() != 0;
            //    db.Orders.CreateUpdate(order);
            //});



            //var searchServiceInstance = new AgroSearch("agrisearch", "F9189208F49AF7C3DFD34E45A89F19E4", "entities");
            //var searchServiceInstance = new AgroSearch("agrosearch", "016DAA5EF1158FEEEE58DA60996D5981", "entities"); 
            //var db = new AgroRepository(new AgroDbArguments
            //{
            //    EndPointUrl = "https://agricola-db.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="
            //});

            //var productsQuery = db.Products.GetProducts();
            //var dbProductOperations = new CommonDbOperations<Product>();
            //List<Product> products = await dbProductOperations.TolistAsync(productsQuery);

            //var executionsQuery = db.ExecutionOrders.GetExecutionOrders();
            //var dbExecutionsOperations = new CommonDbOperations<ExecutionOrder>();
            //List<ExecutionOrder> executions = await dbExecutionsOperations.TolistAsync(executionsQuery);

            //var ordersQuery = db.Orders.GetApplicationOrders();
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);

            //int indice = 0;
            //int porcentaje = 0;
            //int cantidad = products.Count();
            //int percentilCada = cantidad/100;
            //var productsIndex = new List<EntitySearch>();
            //Console.WriteLine("Producto");
            //products.ForEach(producto =>
            //{
            //    indice++;
            //    //if (indice % percentilCada == 0)
            //        Console.WriteLine(indice);
            //    productsIndex.Add(new EntitySearch { Created = DateTime.Now, EntityName = "Product", Id = producto.Id, Name = producto.CommercialName });
            //});

            //indice = 0;
            //porcentaje = 0;
            //cantidad = executions.Count();
            //percentilCada = cantidad/100;
            //var executionsIndex = new List<EntitySearch>();
            //Console.WriteLine("Ejecucion");
            //executions.ForEach(execution =>
            //{
            //    indice++;
            //    //if (indice % percentilCada == 0)
            //        Console.WriteLine(indice);
            //    executionsIndex.Add(new EntitySearch { Created = DateTime.Now, EntityName = "ExecutionOrder", Id = execution.Id, Name = execution.Name, SeasonId = execution.SeasonId, Status = (int)execution.ExecutionStatus });
            //});

            //indice = 0;
            //porcentaje = 0;
            //cantidad = orders.Count();
            //percentilCada = cantidad / 100;
            //var ordersIndex = new List<EntitySearch>();
            //Console.WriteLine("Orden");
            //orders.ForEach(order =>
            //{
            //    indice++;
            //    if (indice % percentilCada == 0)
            //        Console.WriteLine(++porcentaje + "%");
            //    ordersIndex.Add(new EntitySearch { Created = DateTime.Now, EntityName = "ApplicationOrder", Id = order.Id, Name = order.Name, SeasonId = order.SeasonId });
            //});

            //searchServiceInstance.AddEntities(productsIndex);
            //searchServiceInstance.AddEntities(executionsIndex);
            //searchServiceInstance.AddEntities(ordersIndex);



            //CosmosStore<Barrack> store = new CosmosStore<Barrack>(new CosmosStoreSettings("agrodb", "https://agricola-db.documents.azure.com:443/", "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="));
            //await store.RemoveAsync(barrack => !"CIRUELA DURAZNOS KIWIS NECTARINES CEREZAS MANZANA".Split().Contains(barrack.Variety.Specie.Name));

            //var searchServiceInstance = new AgroSearch("agrisearch", "F9189208F49AF7C3DFD34E45A89F19E4", "entities");
            //searchServiceInstance.DeleteEntities(searchServiceInstance.GetSearchFilteredByEntityName(new Filters { EntityName = "ApplicationOrder"},null,null,null,null).Entities.ToList());
            //var ordersQuery = db.Orders.GetApplicationOrders();
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);
            //var ordersIndex = new List<EntitySearch>();
            //int indice = 0;
            //int porcentaje = 0;
            //int cantidad = orders.Count();
            //int percentilCada = cantidad / 100;
            //orders.ForEach(order => {
            //    if (indice++ % percentilCada == 0)
            //        Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    ordersIndex.Add(new EntitySearch { Created = DateTime.Now, EntityName = "ApplicationOrder", Id = order.Id, Name = order.Name, SeasonId = order.SeasonId, Type = order.isPhenological});
            //});
            //searchServiceInstance.AddEntities(ordersIndex);

        }
    }
}
