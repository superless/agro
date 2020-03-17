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
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.external.operations;
using trifenix.agro.search;
using trifenix.agro.search.model;
using trifenix.agro.search.operations;

namespace trifenix.agro.console {
    class Program {

        static async Task Main(string[] args) {
            Console.WriteLine("Hora de inicio: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));
            Stopwatch timer = Stopwatch.StartNew();

            //await SearchHelper.UpdateSearch();

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
            //    EndPointUrl = "https://agricola-jhm.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw=="
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

            #region Script para poblar db

            //Utils
            //var searchServiceInstance = new AgroSearch("agrosearch", "016DAA5EF1158FEEEE58DA60996D5981");
            //searchServiceInstance.DeleteElements(searchServiceInstance.FilterElements<EntitySearch>($"EntityIndex eq {(int)EntityRelated.TRACTOR}"));

            var repo = new MainGenericDb<EntityContainer>(new AgroDbArguments {
                EndPointUrl = "https://agricola-jhm.documents.azure.com:443/",
                NameDb = "agrodb",
                PrimaryKey = "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw=="
            });

            //await repo.CreateUpdate(new EntityContainer { Id = "TGen", Entity = new Tractor { Id = "Tid", Brand = "TGen", Code = "XY" } });
            //End Utils

            //1.BusinessName
            //2.CostCenter
            //3.Season
            //4.Specie
            //5.Variety
            //6.Sector
            //7.PlotLand
            //8.Rootstock
            //9.Barrack
            //10.IngredientCategory
            //11.Ingredient
            //12.ApplicationTarget
            //13.CertifiedEntity
            //14.Product
            //15.Doses
            //16.PhenologicalEvent
            //17.OrderFolder
            //18.PhenologicalPreOrder
            //19.Tractor
            //20.Nebulizer
            //21.Role
            //22.Job
            //23.User
            //24.NotificationEvent
            //25.ApplicationOrder
            //26.ExecutionOrder


            await repo.Store.RemoveAsync(entityContainer => true);

            var entitiesContainers = new List<EntityContainer>();

            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
        /*0*/   new BusinessName { Id = Guid.NewGuid().ToString("N"), Name = "Agro", Rut = "9.876.543-2", Email = "agro@gmail.com", Giro = "Agronomia", Phone = "88884444", WebPage = "www.agro.trifenix.com" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new CostCenter { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda", IdBusinessName = entitiesContainers.ElementAt(0).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Season { Id = Guid.NewGuid().ToString("N"), Current = true, StartDate = new DateTime(2020,1,1), EndDate = new DateTime(2021, 1, 1), IdCostCenter = entitiesContainers.ElementAt(1).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
                new Specie { Id = Guid.NewGuid().ToString("N"), Name = "Ciruela", Abbreviation = "CI" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Variety { Id = Guid.NewGuid().ToString("N"), Name = "Pink Delight", Abbreviation = "PKD", IdSpecie = entitiesContainers.ElementAt(3).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
        /*5*/   new Variety { Id = Guid.NewGuid().ToString("N"), Name = "Early Queen", Abbreviation = "EARQ", IdSpecie = entitiesContainers.ElementAt(3).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Sector { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new PlotLand { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda", IdSector = entitiesContainers.ElementAt(6).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Rootstock { Id = Guid.NewGuid().ToString("N"), Name = "Nemaguard", Abbreviation = "NEMG" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Barrack { Id = Guid.NewGuid().ToString("N"), SeasonId = entitiesContainers.ElementAt(2).Entity.Id, Name = "Cuartel X", Hectares = 1.5, NumberOfPlants = 453, PlantingYear = 2000, IdRootstock = entitiesContainers.ElementAt(8).Entity.Id, IdPlotLand = entitiesContainers.ElementAt(7).Entity.Id, IdVariety = entitiesContainers.ElementAt(4).Entity.Id, IdPollinator = entitiesContainers.ElementAt(5).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
        /*10*/  new IngredientCategory { Id = Guid.NewGuid().ToString("N"), Name = "Insecticida" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Ingredient { Id = Guid.NewGuid().ToString("N"), Name = "Lambda-cihalotrina", idCategory = entitiesContainers.ElementAt(10).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Ingredient { Id = Guid.NewGuid().ToString("N"), Name = "Imidacloprid", idCategory = entitiesContainers.ElementAt(10).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new ApplicationTarget { Id = Guid.NewGuid().ToString("N"), Name = "Control de plaga" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new CertifiedEntity { Id = Guid.NewGuid().ToString("N"), Name = "Union Europea", Abbreviation = "UEA" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
        /*15*/  new Product { Id = Guid.NewGuid().ToString("N"), Name = "Geminis Wp", Brand = "Anasac", IdActiveIngredient = entitiesContainers.ElementAt(12).Entity.Id, KindOfBottle = 0, MeasureType = (MeasureType)1, Quantity =  500 } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
 /*Preguntar*/  new Dose { Id = Guid.NewGuid().ToString("N"), IdProduct = entitiesContainers.ElementAt(15).Entity.Id, IdsApplicationTarget = new string[] { entitiesContainers.ElementAt(13).Entity.Id }, IdSpecies = new string[] { entitiesContainers.ElementAt(3).Entity.Id }, IdVarieties = new string[] { entitiesContainers.ElementAt(4).Entity.Id, entitiesContainers.ElementAt(5).Entity.Id }, ApplicationDaysInterval = 15, DaysToReEntryToBarrack = 5, DosesApplicatedTo = (DosesApplicatedTo)1, DosesQuantityMin =  500, DosesQuantityMax = 800, NumberOfSequentialApplication = 3, WaitingDaysLabel = 25, WaitingToHarvest = new List<WaitingHarvest> { new WaitingHarvest { IdCertifiedEntity = entitiesContainers.ElementAt(14).Entity.Id, WaitingDays = 25} }, WettingRecommendedByHectares = 2000 } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new PhenologicalEvent { Id = Guid.NewGuid().ToString("N"), Name = "Aparicion de flor", StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 7, 1) } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new OrderFolder {  Id = Guid.NewGuid().ToString("N"), IdSpecie = entitiesContainers.ElementAt(3).Entity.Id, IdApplicationTarget = entitiesContainers.ElementAt(13).Entity.Id, IdPhenologicalEvent = entitiesContainers.ElementAt(17).Entity.Id, IdIngredientCategory = entitiesContainers.ElementAt(10).Entity.Id, IdIngredient = entitiesContainers.ElementAt(12).Entity.Id } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
 /*Preguntar*/  new PreOrder { Id = Guid.NewGuid().ToString("N"), Name = "Eulia", OrderFolderId = entitiesContainers.ElementAt(18).Entity.Id, IdIngredient = entitiesContainers.ElementAt(12).Entity.Id, PreOrderType = (PreOrderType)1, BarracksId = new string[] { entitiesContainers.ElementAt(9).Entity.Id } } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
        /*20*/  new Tractor { Id = Guid.NewGuid().ToString("N"), Brand = "John Deere", Code = "JDT" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
                new Nebulizer { Id = Guid.NewGuid().ToString("N"), Brand = "Lerpain", Code = "LRP" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Role { Id = Guid.NewGuid().ToString("N"), Name = "Administrador" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Role { Id = Guid.NewGuid().ToString("N"), Name = "Aplicador" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new Job { Id = Guid.NewGuid().ToString("N"), Name = "Administrador" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
        /*25*/  new User { Id = Guid.NewGuid().ToString("N"), Name = "Cristian Rojas", Email = "cristian.rojas@alumnos.uv.cl", Rut = "19.193.382-6", IdJob = entitiesContainers.ElementAt(24).Entity.Id, IdsRoles = new List<string> { entitiesContainers.ElementAt(22).Entity.Id, entitiesContainers.ElementAt(23).Entity.Id }, ObjectIdAAD = "d273305e-9a05-4bbb-9bfc-ae724610b93a" } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new NotificationEvent { Id = Guid.NewGuid().ToString("N"), Description = "", IdBarrack = entitiesContainers.ElementAt(9).Entity.Id, NotificationType = (NotificationType)1, IdPhenologicalEvent = entitiesContainers.ElementAt(17).Entity.Id, PicturePath = "https://agricolablob.blob.core.windows.net/contenedor013dae60-0e06-43bc-a5a9-f357c4e63de0/8209d00d-6fe0-45dd-b949-801db88b53c1.jpg", Created = DateTime.Today } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
                new ApplicationOrder { Id = Guid.NewGuid().ToString("N"), Name = "Aplicacion de insecticida en Ciruela", OrderType = (OrderType)1, IdsPhenologicalPreOrder = new string[] { entitiesContainers.ElementAt(19).Entity.Id }, Barracks = new BarrackOrderInstance[] { new BarrackOrderInstance { IdBarrack = entitiesContainers.ElementAt(9).Entity.Id, IdNotificationEvents = new string[] { entitiesContainers.ElementAt(26).Entity.Id } } }, DosesOrder = new DosesOrder[] { new DosesOrder { IdDoses = entitiesContainers.ElementAt(16).Entity.Id, QuantityByHectare = 1350 } }, Wetting = 2000, InitDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 7, 1) } });
            entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
 /*Preguntar*/  new ExecutionOrder { Id = Guid.NewGuid().ToString("N"), IdOrder = entitiesContainers.ElementAt(27).Entity.Id, IdUserApplicator = entitiesContainers.ElementAt(25).Entity.Id, DosesOrder = new DosesOrder[] { new DosesOrder { IdDoses = entitiesContainers.ElementAt(16).Entity.Id, QuantityByHectare = 1350 } }, IdNebulizer = entitiesContainers.ElementAt(21).Entity.Id, IdTractor = entitiesContainers.ElementAt(20).Entity.Id, InitDate = new DateTime(2020, 5, 10), EndDate = new DateTime(2020, 6, 25) } });

            foreach (EntityContainer entityContainer in entitiesContainers)
                await repo.CreateUpdate(entityContainer, false);

            #endregion

            timer.Stop();
            Console.WriteLine("Hora de termino: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));
            Console.WriteLine("Tiempo transcurrido: {0}", timer.Elapsed.ToString("hh\\:mm\\:ss"));

        }

    }

}