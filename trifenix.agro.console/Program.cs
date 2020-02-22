using Cosmonaut;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.operations;
using trifenix.agro.search;
using trifenix.agro.search.model;

namespace trifenix.agro.console {
    class Program {
        static async Task Main(string[] args) {
            Stopwatch timer = Stopwatch.StartNew();
            Console.WriteLine("Hora de inicio: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));

            #region Reflexion
            //Console.WriteLine("typeof(Barrack).Name:            " + typeof(Barrack).Name);
            //Console.WriteLine("typeof(Barrack).FullName:        " + typeof(Barrack).FullName);
            //Console.WriteLine("typeof(Barrack).ReflectedType:   " + typeof(Barrack).ReflectedType);
            //Console.WriteLine("typeof(Barrack).ToString():      " + typeof(Barrack).ToString());
            //Console.WriteLine("\ntypeof(Barrack).GetProperties().ToList().ForEach(): ");
            //typeof(Barrack).GetProperties().ToList().ForEach(Propiedad => Console.WriteLine("Propiedad.PropertyType.Name + Propiedad.Name: " + Propiedad.PropertyType.Name + " " + Propiedad.Name));
            #endregion

            #region CreacionDeAgroRepository
            //var db = new AgroRepository(new AgroDbArguments
            //{
            //    EndPointUrl = "https://agricola-db.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="
            //});
            #endregion

            #region ObtencionDeElementos (Productos, Especies, Variedades, Cuarteles, Execuciones y Ordenes)
            //var productsQuery = db.Products.GetProducts();
            //var dbProductOperations = new CommonDbOperations<Product>();
            //List<Product> products = await dbProductOperations.TolistAsync(productsQuery);

            //var barracksQuery = db.Barracks.GetBarracks();
            //var dbBarrackOperations = new CommonDbOperations<Barrack>();
            //List<Barrack> barracks = await dbBarrackOperations.TolistAsync(barracksQuery);

            //var speciesQuery = db.Species.GetSpecies();
            //var dbSpecieOperations = new CommonDbOperations<Specie>();
            //List<Specie> species = await dbSpecieOperations.TolistAsync(speciesQuery);

            //var varietiesQuery = db.Varieties.GetVarieties();
            //var dbVarietyOperations = new CommonDbOperations<Variety>();
            //List<Variety> varieties = await dbVarietyOperations.TolistAsync(varietiesQuery);

            //var executionsQuery = db.ExecutionOrders.GetExecutionOrders();
            //var dbExecutionsOperations = new CommonDbOperations<ExecutionOrder>();
            //List<ExecutionOrder> executions = await dbExecutionsOperations.TolistAsync(executionsQuery);

            //var ordersQuery = db.Orders.GetApplicationOrders();
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);
            #endregion

            #region ObtenElementosDbConFiltro
            //var ordersQuery = db.Orders.GetApplicationOrders().Where(order => order.PhenologicalPreOrders.Any(ph => ph.Id.Equals("79d69acc62a8488f914bab9d1333a80e")));
            //var dbOrderOperations = new CommonDbOperations<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);
            #endregion

            #region ActualizaElementosDb
            //int indice = 0;
            //float porcentaje = 0;
            //int cantidad = orders.Count;
            //float incremento = (float)100 / cantidad;
            //Console.WriteLine("Existen " + cantidad + " ApplicationOrder sin actualizar.");
            //orders.ToList().ForEach(order => {
            //    Console.WriteLine(Math.Round(porcentaje, 2) + "% -> Indice: " + ++indice);
            //    porcentaje += incremento;
            //    order.InnerCorrelative = db.ExecutionOrders.GetExecutionOrders(order.Id).Count() + 1;
            //    db.Orders.CreateUpdate(order);
            //});
            #endregion

            #region CreacionDeIndiceAzureSearch
            //int indice = 0;
            //int porcentaje = 0;
            //int cantidad = products.Count;
            //int percentilCada = cantidad / 100;
            //var productsIndex = new List<EntitySearch>();
            //Console.WriteLine("Producto");
            //products.ForEach(producto =>
            //{
            //    //if (indice++ % percentilCada == 0)
            //    //    Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    Console.WriteLine("Indice: " + ++indice);
            //    productsIndex.Add(new EntitySearch { Id = producto.Id, Created = DateTime.Now, EntityName = "Product", Name = producto.CommercialName });
            //});

            //indice = 0;
            //porcentaje = 0;
            //cantidad = barracks.Count;
            //percentilCada = cantidad / 100;
            //var barracksIndex = new List<EntitySearch>();
            //Console.WriteLine("Cuartel");
            //barracks.ForEach(barrack =>
            //{
            //    //if (indice++ % percentilCada == 0)
            //    //    Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    Console.WriteLine("Indice: " + ++indice);
            //    barracksIndex.Add(new EntitySearch { Created = DateTime.Now, EntityName = "Barrack", Id = barrack.Id, Name = barrack.Name, SeasonId = barrack.SeasonId, Specie = barrack.Variety.Specie.Abbreviation });
            //});

            //indice = 0;
            //porcentaje = 0;
            //cantidad = executions.Count;
            //percentilCada = cantidad / 100;
            //var executionsIndex = new List<EntitySearch>();
            //Console.WriteLine("Ejecucion");
            //executions.ForEach(execution =>
            //{
            //    //if (indice++ % percentilCada == 0)
            //    //    Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    Console.WriteLine("Indice: " + ++indice);
            //    executionsIndex.Add(new EntitySearch { Id = execution.Id, Created = DateTime.Now, SeasonId = execution.SeasonId, EntityName = "ExecutionOrder", Name = execution.Name, Specie = execution.Order.Barracks.FirstOrDefault().Barrack.Variety.Specie.Abbreviation, Status = (int)execution.ExecutionStatus });
            //});

            //indice = 0;
            //porcentaje = 0;
            //cantidad = orders.Count;
            //percentilCada = cantidad / 100;
            //var ordersIndex = new List<EntitySearch>();
            //Console.WriteLine("Orden");
            //orders.ForEach(order =>
            //{
            //    if (indice++ % percentilCada == 0)
            //        Console.WriteLine(porcentaje++ + "% -> Indice: " + indice);
            //    ordersIndex.Add(new EntitySearch { Id = order.Id, Created = DateTime.Now, SeasonId = order.SeasonId, EntityName = "ApplicationOrder", Name = order.Name, Specie = order.Barracks.FirstOrDefault().Barrack.Variety.Specie.Abbreviation, Type = order.IsPhenological });
            //});

            //var searchServiceInstance = new AgroSearch("agrisearch", "F9189208F49AF7C3DFD34E45A89F19E4", "entities");
            //searchServiceInstance.AddEntities(productsIndex);
            //searchServiceInstance.AddEntities(barracksIndex);
            //searchServiceInstance.AddEntities(executionsIndex);
            //searchServiceInstance.AddEntities(ordersIndex);
            #endregion

            #region EliminarElementosDeIndicePorEntityName
            //var searchServiceInstance = new AgroSearch("agrisearch", "F9189208F49AF7C3DFD34E45A89F19E4", "entities");
            //searchServiceInstance.DeleteEntities(searchServiceInstance.GetSearchFilteredByEntityName(new Filters { EntityName = "Barrack" }, null, null, null, null).Entities.ToList());
            #endregion

            #region ScriptEncuentraInconsistencias
            //List<string> SpecieNames = species.Select(sp => sp.Name).ToList();
            //List<string> SpecieAbb = species.Select(sp => sp.Abbreviation).ToList();

            //List<string> VarietyNames = varieties.Select(sp => sp.Name).ToList();
            //List<string> VarietyAbb = varieties.Select(sp => sp.Abbreviation).ToList();

            //Specie specie;
            //Variety variety;
            //List<Barrack> barracks;

            //int indice = 0;
            //float porcentaje = 0;
            //int cantidad = orders.Count;
            //int percentilCada = cantidad / 100;
            //List<string> barrackErrors = new List<string>();
            //List<string> orderErrors = new List<string>();
            //List<string> errors = new List<string>();
            //Console.WriteLine("Total de ordenes: " + orders.Count);
            //int orderWithProblem = 0;
            //orders.ForEach(order =>
            //{
            //    porcentaje += 0.05F;
            //    Console.WriteLine(Math.Round(porcentaje,2) + "% -> Indice: " + ++indice);
            //    orderErrors = new List<string> { "ApplicationOrder: " + order.Id };
            //    barracks = order.Barracks.Select(barrackInstance => barrackInstance.Barrack).ToList();
            //    barracks.ForEach(barrack =>
            //    {
            //        barrackErrors = new List<string> { "Barrack: " + barrack.Id };
            //        variety = db.Varieties.GetVariety(barrack.Variety.Id).Result;
            //        if (variety == null)
            //            barrackErrors.Add("No existe variedad con este id");
            //        else
            //        {
            //            if (!VarietyNames.Contains(barrack.Variety.Name))
            //                barrackErrors.Add("Nombre de variedad no existe");
            //            if (!VarietyAbb.Contains(barrack.Variety.Abbreviation))
            //                barrackErrors.Add("Abreviacion de variedad no existe");
            //        }
            //        specie = db.Species.GetSpecie(barrack.Variety.Specie.Id).Result;
            //        if (specie == null)
            //            barrackErrors.Add("No existe especie con este id");
            //        else
            //        {
            //            if (!SpecieNames.Contains(barrack.Variety.Specie.Name))
            //                barrackErrors.Add("Nombre de especie no existe");
            //            if (!SpecieAbb.Contains(barrack.Variety.Specie.Abbreviation))
            //                barrackErrors.Add("Abreviacion de especie no existe");
            //        }
            //        if (barrackErrors.Count > 1)
            //            orderErrors = orderErrors.Concat(barrackErrors).ToList();
            //    });
            //    if (orderErrors.Count > 1)
            //    {
            //        orderWithProblem++;
            //        errors = errors.Concat(orderErrors).ToList();
            //    }
            //});
            //File.WriteAllLines("C:/Users/Trifenix/Desktop/CompleteLog.txt", errors);
            //Console.WriteLine("\nOrden con problemas: (" + orderWithProblem + ")");
            //errors.ForEach(error => Console.WriteLine(error));
            #endregion

            #region EliminaElementosDbCondicionalmente
            //CosmosStore<Barrack> store = new CosmosStore<Barrack>(new CosmosStoreSettings("agrodb", "https://agricola-db.documents.azure.com:443/", "1hrGHt13NgzgOTahFZXDmtugRg5rld9Y9TstCNXg4arZbdOlK4I6h2EOD51Ezgpxe5wsQUxGKaODgET1LSsS4Q=="));
            //await store.RemoveAsync(barrack => !"CIRUELA DURAZNOS KIWIS NECTARINES CEREZAS MANZANA".Split().Contains(barrack.Variety.Specie.Name));
            #endregion

            #region LecturaLog (Actualizar entidades erroneas)
            //List<string> lines = File.ReadAllLines(@"C:\Users\Trifenix\Desktop\Log - ApplicationOrder.txt").Where(line => line.Contains("ApplicationOrder")).Select(line => line.Split()[1]).ToList();
            //foreach (string line in lines) {
            //    Console.WriteLine(line);
            //}
            //Console.WriteLine("Total de Ordenes: " + lines.Count);
            //var season = await db.Seasons.GetCurrentSeason();
            //var agro = new AgroManager(db, season?.Id, null, null, null, null);
            //agro.ApplicationOrders.UpdateOrder(lines, db);
            #endregion

            timer.Stop();
            Console.WriteLine("Hora de termino: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));
            Console.WriteLine("Tiempo transcurrido: {0}", timer.Elapsed.ToString("hh\\:mm\\:ss"));

        }
    }

}