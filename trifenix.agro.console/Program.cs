using Cosmonaut;
using Cosmonaut.Response;
using Microsoft.Spatial;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.model.local;
using trifenix.agro.external.operations;
using trifenix.agro.search.operations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro_model;
using static trifenix.connect.util.Mdm.Reflection;

namespace trifenix.agro.console
{

    class Program {

        public static Task<CosmosMultipleResponse<T>> RemoveAsync<T>(CosmosStoreSettings StoreSettings) where T : class => new CosmosStore<T>(StoreSettings).RemoveAsync(entity => true);

        public static async Task RenewClientIds(AgroManager agro, Type dbType) {
            await agro.GetOperationByDbType(dbType).RenewClientIds();
        }

        static async Task Main(string[] args) {

            Environment.SetEnvironmentVariable("clientSecret", "B._H_uAwEdg7K1FzVboS3S/oF4IKNbtf");
            Environment.SetEnvironmentVariable("clientID", "34d9266f-43f9-4fb2-8cdd-ae21be551342");
            Environment.SetEnvironmentVariable("tenantID", "13f71027-8389-436e-bdaf-7bd34382fbff");

            var agroDbArguments = new AgroDbArguments { EndPointUrl = "https://agricola-jhm.documents.azure.com:443/", NameDb = "agrodb", PrimaryKey = "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw==" };
            var search = new AgroSearch<GeographyPoint>("agrosearch", "016DAA5EF1158FEEEE58DA60996D5981");
            var agro = new AgroManager(agroDbArguments, null, null, null, search, null, false);

            var assm = typeof(BusinessName).Assembly;
            var types = new[] { typeof(UserApplicator) };
            //var types = assm.GetTypes().Where(type => type.GetProperty("CosmosEntityName") != null && !(new[] { typeof(EntityContainer), typeof(User), typeof(Comment) }).Contains(type)).ToList();

            IEnumerable<Task> tasks;
            ConcurrentBag<object> bag;

            //bag = new ConcurrentBag<object>();
            //tasks = types.Select(async type =>
            //    bag.Add(RenewClientIds(agro, type))
            //);
            //await Task.WhenAll(tasks);

            foreach (var type in types) {
                try {
                    await RenewClientIds(agro, type);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            //search.EmptyIndex<EntitySearch>("entities");
            search.DeleteElements($"entityIndex/any(index: index eq {(int)EntityRelated.USER})");
            await search.GenerateIndex(agro);

            
            return;

            Console.WriteLine("Hora de inicio: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));
            Stopwatch timer = Stopwatch.StartNew();

            // Inicio Script

            Environment.SetEnvironmentVariable("clientSecret", "B._H_uAwEdg7K1FzVboS3S/oF4IKNbtf");
            Environment.SetEnvironmentVariable("clientID", "34d9266f-43f9-4fb2-8cdd-ae21be551342");
            Environment.SetEnvironmentVariable("tenantID", "13f71027-8389-436e-bdaf-7bd34382fbff");

            //Aquí defino si se vaciará CosmosDb, Index Search, ambos o ninguno
            bool vaciarCosmosDb = false, vaciarSearch = false, vaciarAmbos = true;

            //var search = new AgroSearch("search-agro-produccion", "A256D7F2BD95055691460D358CA870BA");
            //if (vaciarAmbos || vaciarSearch)
            //    search.EmptyIndex<EntitySearch>("entities");

            //var agroDbArguments = new AgroDbArguments { EndPointUrl = "https://agro-jhm-produccion.documents.azure.com:443/", NameDb = "agrodb", PrimaryKey = "sZTarTcwaiO2LUghxZEuIGd9FXIZ7ziqkVAbVmJWBucREVQ3YWYr5Jke7E1gR9UlJUkdYOLHZWteiuKE37LbLA==" };

            //IEnumerable<Task> tasks;
            //ConcurrentBag<object> bag;

            if (vaciarAmbos || vaciarCosmosDb) {
                var storeSettings = new CosmosStoreSettings(agroDbArguments.NameDb, agroDbArguments.EndPointUrl, agroDbArguments.PrimaryKey);
                bag = new ConcurrentBag<object>();
                //var assm = typeof(BusinessName).Assembly;
                //var types = assm.GetTypes().Where(type => type.GetProperty("CosmosEntityName") != null).ToList();
                tasks = types.Select(async type => {
                    var response = typeof(Program).GetMethod("RemoveAsync").MakeGenericMethod(type).Invoke(null, new object[] { storeSettings });
                    bag.Add(response);
                });
                await Task.WhenAll(tasks);
            }

            var elements = new List<object> {
                new BusinessName { Name = "Agrícola Juan Henriquez Marich", Giro = "Agronomía", WebPage = "www.aresa.trifenix.io" }, //0
                new BusinessName { Name = "Agrícola Azapa Ltda.", Giro = "Agronomía", WebPage = "www.aresa.trifenix.io" },
                new BusinessName { Name = "Agrícola El Delirio", Giro = "Agronomía", WebPage = "www.aresa.trifenix.io" },
                new CostCenter { Name = "Esmeralda", IdBusinessName = "0" },
                new CostCenter { Name = "Lechería", IdBusinessName = "0" },
                new CostCenter { Name = "Azapa", IdBusinessName = "1" },
                new CostCenter { Name = "El Delirio", IdBusinessName = "2" },
                new Season { Current = true, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2021, 4, 30), IdCostCenter = "3" },
                new Season { Current = true, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2021, 4, 30), IdCostCenter = "4" },
                new Season { Current = true, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2021, 4, 30), IdCostCenter = "5" },
                new Season { Current = true, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2021, 4, 30), IdCostCenter = "6" }, //10
                new Specie { Name = "Cereza", Abbreviation = "CE" },
                new Specie { Name = "Nectarín", Abbreviation = "NE" },
                new Specie { Name = "Durazno", Abbreviation = "DU" },
                new Specie { Name = "Ciruela", Abbreviation = "CI" },
                new Specie { Name = "Manzana", Abbreviation = "MA" },
                new Specie { Name = "Kiwi", Abbreviation = "KI" },
                new Specie { Name = "Pera", Abbreviation = "PE" },
                new Variety { Name = "Royal Dawn", Abbreviation = "C-14", IdSpecie = "11" },    //Cereza
                new Variety { Name = "Van", Abbreviation = "VN", IdSpecie = "11" },
                new Variety { Name = "New Star", Abbreviation = "NSTR", IdSpecie = "11" },      //20
                new Variety { Name = "Bing", Abbreviation = "BNG", IdSpecie = "11" },
                new Variety { Name = "Lapins", Abbreviation = "LPNS", IdSpecie = "11" },
                new Variety { Name = "Summit", Abbreviation = "SMMT", IdSpecie = "11" },
                new Variety { Name = "Santina", Abbreviation = "SNTN", IdSpecie = "11" },
                new Variety { Name = "Royal Lynn", Abbreviation = "RYLN", IdSpecie = "11" },
                new Variety { Name = "Royal Hazel", Abbreviation = "RYHZL", IdSpecie = "11" },
                new Variety { Name = "Sweet Heart", Abbreviation = "SWHRT", IdSpecie = "11" },
                new Variety { Name = "Cristalina", Abbreviation = "CRSTLN", IdSpecie = "11" },
                new Variety { Name = "Regina", Abbreviation = "RGN", IdSpecie = "11" },
                new Variety { Name = "Summer Bright", Abbreviation = "SMRBRGT", IdSpecie = "12" },  //Nectarin      //30
                new Variety { Name = "July Red", Abbreviation = "JLRD", IdSpecie = "12" },
                new Variety { Name = "Flamekist", Abbreviation = "FLMKST", IdSpecie = "12" },
                new Variety { Name = "Artic Snow", Abbreviation = "ATSNW", IdSpecie = "12" },
                new Variety { Name = "Firebrite", Abbreviation = "FRBRT", IdSpecie = "12" },
                new Variety { Name = "August Red", Abbreviation = "AGTRD", IdSpecie = "12" },
                new Variety { Name = "Bright Pearl", Abbreviation = "BRGTPRL", IdSpecie = "12" },
                new Variety { Name = "August Pearl", Abbreviation = "AGTPRL", IdSpecie = "12" },
                new Variety { Name = "Andes Nec Uno", Abbreviation = "ADNC1", IdSpecie = "12" },
                new Variety { Name = "Andes Nec Dos", Abbreviation = "ADNC2", IdSpecie = "12" },
                new Variety { Name = "Super Queen", Abbreviation = "SPRQN", IdSpecie = "12" },      //40
                new Variety { Name = "NE-289", Abbreviation = "NE-289", IdSpecie = "12" },
                new Variety { Name = "Isi White", Abbreviation = "IWHT", IdSpecie = "12" },
                new Variety { Name = "Candy White", Abbreviation = "CNDWHT", IdSpecie = "12" },
                new Variety { Name = "Magnum Red", Abbreviation = "MGNRD", IdSpecie = "12" },
                new Variety { Name = "Ruby Red", Abbreviation = "RBRD", IdSpecie = "12" },
                new Variety { Name = "Venus", Abbreviation = "VNS", IdSpecie = "12" },
                new Variety { Name = "Sunrise", Abbreviation = "SNRS", IdSpecie = "12" },
                new Variety { Name = "Cal Red", Abbreviation = "CLRD", IdSpecie = "13" },   //Durazno
                new Variety { Name = "Elegant Lady", Abbreviation = "ELGNTLDY", IdSpecie = "13" },
                new Variety { Name = "Sweet September", Abbreviation = "SWTSPTMBR", IdSpecie = "13" },      //50
                new Variety { Name = "Rose Pearl", Abbreviation = "RSPRL", IdSpecie = "13" },
                new Variety { Name = "Royal Glory", Abbreviation = "RYLGRY", IdSpecie = "13" },
                new Variety { Name = "Flavorcrest", Abbreviation = "FLVRCST", IdSpecie = "13" },
                new Variety { Name = "Robin Neil", Abbreviation = "RBNNL", IdSpecie = "13" },
                new Variety { Name = "Scarlet Snow", Abbreviation = "SCLTSNW", IdSpecie = "13" },
                new Variety { Name = "September Sun", Abbreviation = "SPTMBRSN", IdSpecie = "13" },
                new Variety { Name = "White Lady", Abbreviation = "WHTLDY", IdSpecie = "13" },
                new Variety { Name = "D 23", Abbreviation = "D23", IdSpecie = "13" },
                new Variety { Name = "DU 88", Abbreviation = "D88", IdSpecie = "13" },
                new Variety { Name = "Du 600", Abbreviation = "D600", IdSpecie = "13" },      //60
                new Variety { Name = "Pink Delight", Abbreviation = "PNKDLGHT", IdSpecie = "14" },  //Ciruela
                new Variety { Name = "Owen T", Abbreviation = "OWNT", IdSpecie = "14" },
                new Variety { Name = "Black Amber", Abbreviation = "BLKAMBR", IdSpecie = "14" },
                new Variety { Name = "Angeleno", Abbreviation = "ANGLNO", IdSpecie = "14" },
                new Variety { Name = "RR1", Abbreviation = "RR1", IdSpecie = "14" },
                new Variety { Name = "Fortune", Abbreviation = "FRTN", IdSpecie = "14" },
                new Variety { Name = "Friar", Abbreviation = "FRR", IdSpecie = "14" },
                new Variety { Name = "Deep Blue", Abbreviation = "DPBLE", IdSpecie = "14" },
                new Variety { Name = "Autumn Pride", Abbreviation = "ATMNPRD", IdSpecie = "14" },
                new Variety { Name = "Hiromi Red", Abbreviation = "HRMRD", IdSpecie = "14" },      //70
                new Variety { Name = "Betty Ann", Abbreviation = "BTYAN", IdSpecie = "14" },
                new Variety { Name = "Larry Ann", Abbreviation = "LRYAN", IdSpecie = "14" },
                new Variety { Name = "Nubiana", Abbreviation = "NBNA", IdSpecie = "14" },
                new Variety { Name = "Early Queen", Abbreviation = "ELYQN", IdSpecie = "14" },
                new Variety { Name = "Royal Zee", Abbreviation = "RYLZE", IdSpecie = "14" },
                new Variety { Name = "Royal Diamond", Abbreviation = "RYLDMND", IdSpecie = "14" },
                new Variety { Name = "Roysum", Abbreviation = "RYSM", IdSpecie = "14" },
                new Variety { Name = "Zafiro", Abbreviation = "ZFR", IdSpecie = "14" },
                new Variety { Name = "Candy Stripe", Abbreviation = "CDYSTPE", IdSpecie = "14" },
                new Variety { Name = "Black Kat", Abbreviation = "BLCKKT", IdSpecie = "14" },      //80
                new Variety { Name = "Granny Smith", Abbreviation = "GRNYSMTH", IdSpecie = "15" },  //Manzana
                new Variety { Name = "Fuji", Abbreviation = "FJ", IdSpecie = "15" },
                new Variety { Name = "Starkrimson", Abbreviation = "STRKMSN", IdSpecie = "15" },
                new Variety { Name = "Hayward", Abbreviation = "HYWRD", IdSpecie = "16" },  //Kiwi
                new Variety { Name = "Winter Nelis", Abbreviation = "WNTRNLS", IdSpecie = "17" },   //Pera
                new Variety { Name = "Packham", Abbreviation = "PCKHM", IdSpecie = "17" },
                new Variety { Name = "Red Sensation", Abbreviation = "RDSNSTN", IdSpecie = "17" },
                new Sector { Name = "Azapa" },
                new Sector { Name = "El Delirio" },
                new Sector { Name = "Lechería" },      //90
                new Sector { Name = "Esmeralda" },
                new PlotLand { Name = "Azapa", IdSector = "88" },
                new PlotLand { Name = "Camarico", IdSector = "91" },
                new PlotLand { Name = "Zañartu", IdSector = "91" },
                new PlotLand { Name = "Packing", IdSector = "91" },
                new Rootstock { Name = "Nemaguard", Abbreviation = "NEMG" },
                new Rootstock { Name = "Maxma 14", Abbreviation = "MXMA14" },
                new Rootstock { Name = "Mariana 2624", Abbreviation = "MRNA2624" },
                new Rootstock { Name = "Franco", Abbreviation = "FRNCO" },
                new Rootstock { Name = "Estaca", Abbreviation = "ESTCA" },      //100
            //new Barrack { SeasonId = "2", Name = "Cuartel X", Hectares = 1.5, NumberOfPlants = 453, PlantingYear = 2000, IdRootstock = "8", IdPlotLand = "7", IdVariety = "4", IdPollinator = "5" },
                new IngredientCategory { Name = "Insecticida" },
                new Ingredient { Name = "Lambda-cihalotrina", idCategory = "101" },
                new Ingredient { Name = "Imidacloprid", idCategory = "101" },
                new ApplicationTarget { Name = "Control de plaga" },
                new CertifiedEntity { Name = "Union Europea", Abbreviation = "UEA" },
                new Product { Name = "Geminis Wp", Brand = "Anasac", IdActiveIngredient = "102", KindOfBottle = 0, MeasureType = (MeasureType)1, Quantity = 500 },
                new Dose { IdProduct = "106", IdsApplicationTarget = new string[] { "104" }, IdSpecies = new string[] { "14" }, IdVarieties = new string[] { "61", "68" }, ApplicationDaysInterval = 15, HoursToReEntryToBarrack = 5, DosesApplicatedTo = (DosesApplicatedTo)1, DosesQuantityMin = 500, DosesQuantityMax = 800, NumberOfSequentialApplication = 3, WaitingDaysLabel = 25, WaitingToHarvest = new List<WaitingHarvest> { new WaitingHarvest { IdCertifiedEntity = "105", WaitingDays = 25 } }, WettingRecommendedByHectares = 2000 },
                new PhenologicalEvent { Name = "Aparicion de flor", StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 7, 1) },
                new OrderFolder { IdSpecie = "14", IdApplicationTarget = "104", IdPhenologicalEvent = "108", IdIngredientCategory = "101", IdIngredient = "102" },
            //new PreOrder { Name = "Eulia", OrderFolderId = "18", IdIngredient = "12", PreOrderType = (PreOrderType)1, BarracksId = new string[] { "9" } },
                
                new Tractor { Brand = "John Deere", Code = "JD" },  //110
                new Tractor { Brand = "Kubota", Code = "KBT" },
                new Tractor { Brand = "New Holland TF75", Code = "NHTF" },
                new Nebulizer { Brand = "Omnibus BR-CN116B", Code = "OB" },
                new Nebulizer { Brand = "Omron Comp A.I.R. NE-C28P", Code = "OCAN" },
                new Nebulizer { Brand = "Unigreem 2000", Code = "UGRM" },
                new Role { Name = "Administrador" },
                new Role { Name = "Aplicador" },
                new Role { Name = "Supervisor" },
                new Job { Name = "Administrador" },
                new Job { Name = "Bodeguero" }, //120
                new Job { Name = "Aplicador" },
                //TODO: Debo modificar estructura User -> UserApplicator!
                new UserApplicator { Name = "Alejandro Iglesias", Email = "alejandro.iglesias@trifenix.com", Rut = "19.956.606-7", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "ba7e86c8-6c2d-491d-bb2e-0dd39fdf5dc1" },
                new UserApplicator { Name = "Carolina Aranda", Email = "control@aresa.cl", Rut = "12.112.781-4", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "45c92c5b-51e5-4498-ba92-90d4c53b0b28" },
                new UserApplicator { Name = "Cristian Rojas", Email = "cristian.rojas@alumnos.uv.cl", Rut = "19.193.382-6", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "8a59e2bc-aeb3-4ad7-a1e3-ccff9e3a233d" },
                new UserApplicator { Name = "Jennifer San Martin Lecaros", Email = "contraparte@aresa.cl", Rut = "24.392.653-k", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "ab5fb6f4-b0d0-4b49-a19a-2db78ad7b715" },
                new UserApplicator { Name = "Luis Rivera", Email = "lrivera@aresa.cl", Rut = "6.753.025-k", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "cd1bf5ac-8d84-44ae-bfc6-74fbf85e8e84" },
                new UserApplicator { Name = "Pilar Concha", Email = "pilarconcha@aresa.cl", Rut = "12.577.449-0", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "4f166348-a862-48ed-b74d-f6f0669f17f7" },
                new UserApplicator { Name = "Clemente Henriquez", Email = "c.henriquez@aresa.cl", Rut = "17.149.660-8", IdJob = "119", IdsRoles = new List<string> { "116" }, ObjectIdAAD = "cde778aa-01f1-4f6c-920a-0b790f3234c7" },
            };

            var guids = new List<string>();

            elements.ForEach(element => guids.Add(Guid.NewGuid().ToString("N")));
            int position = 0;
            elements.ForEach(element => {
                element.GetType().GetProperties().Where(prop => prop.Name.ToLower().Contains("id") && !prop.Name.Equals("ClientId") && !prop.Name.Equals("ObjectIdAAD")).ToList().ForEach(
                    prop => {
                        if (HasValue(prop.GetValue(element))) {
                            if (!IsEnumerableProperty(prop)) {
                                var indexString = prop.GetValue(element).ToString();
                                var index = int.Parse(indexString);
                                prop.SetValue(element, guids[index]);
                            }
                            else {
                                //Obtengo el IEnumerable(Array o List), valor de propiedad de tipo id, para luego recorrerlo y generar nueva lista con guids reemplazados
                                var listGuids = ((IEnumerable<object>)prop.GetValue(element)).Select(id => guids[int.Parse(id.ToString())]);
                                prop.SetValue(element, prop.PropertyType.IsArray ? listGuids.ToArray() : (object)listGuids.ToList());
                            }
                        }
                        else if (prop.Name.Equals("Id"))
                            prop.SetValue(element, guids[position]);
                    }
                );
                position++;
            });

            //var agro = new AgroManager(agroDbArguments, null, null, null, search, null, false);

            bag = new ConcurrentBag<object>();
            tasks = elements.Select(async element => {
                var response = await agro.GetOperationByDbType(element.GetType()).Save(element as dynamic);
                bag.Add(response);
            });
            await Task.WhenAll(tasks);

            await search.GenerateIndex(agro);
            //var search = new AgroSearch("agrosearch", "016DAA5EF1158FEEEE58DA60996D5981");
            //var entity = search.GetEntity(EntityRelated.PHENOLOGICAL_EVENT, "e158ffd87a27410ab1b0b73cda2ecccb");
            //entity.Num64Properties = new Num64Property[] { };
            //search.AddElements(new List<EntitySearch> { entity });

            // Fin Script



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
            //var dbProductOperations = GetCommonDbOp<Product>();
            //List<Product> products = await dbProductOperations.TolistAsync(productsQuery);

            //var barracksQuery = db.Barracks.GetBarracks();
            //var dbBarrackOperations = GetCommonDbOp<Barrack>();
            //List<Barrack> barracks = await dbBarrackOperations.TolistAsync(barracksQuery);

            //var speciesQuery = db.Species.GetSpecies();
            //var dbSpecieOperations = GetCommonDbOp<Specie>();
            //List<Specie> species = await dbSpecieOperations.TolistAsync(speciesQuery);

            //var varietiesQuery = db.Varieties.GetVarieties();
            //var dbVarietyOperations = GetCommonDbOp<Variety>();
            //List<Variety> varieties = await dbVarietyOperations.TolistAsync(varietiesQuery);

            //var executionsQuery = db.ExecutionOrders.GetExecutionOrders();
            //var dbExecutionsOperations = GetCommonDbOp<ExecutionOrder>();
            //List<ExecutionOrder> executions = await dbExecutionsOperations.TolistAsync(executionsQuery);

            //var ordersQuery = db.Orders.GetApplicationOrders();
            //var dbOrderOperations = GetCommonDbOp<ApplicationOrder>();
            //List<ApplicationOrder> orders = await dbOrderOperations.TolistAsync(ordersQuery);
            #endregion

            #region ObtenElementosDbConFiltro
            //var ordersQuery = db.Orders.GetApplicationOrders().Where(order => order.PhenologicalPreOrders.Any(ph => ph.Id.Equals("79d69acc62a8488f914bab9d1333a80e")));
            //var dbOrderOperations = GetCommonDbOp<ApplicationOrder>();
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

            //var repo = new MainGenericDb<EntityContainer>(new AgroDbArguments {
            //    EndPointUrl = "https://agricola-jhm.documents.azure.com:443/",
            //    NameDb = "agrodb",
            //    PrimaryKey = "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw=="
            //});

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


            //           await repo.Store.RemoveAsync(entityContainer => true);

            //           var entitiesContainers = new List<EntityContainer>();

            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //       /*0*/   new BusinessName { Id = Guid.NewGuid().ToString("N"), Name = "Agro", Rut = "9.876.543-2", Email = "agro@gmail.com", Giro = "Agronomia", Phone = "88884444", WebPage = "www.agro.trifenix.com" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new CostCenter { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda", IdBusinessName = entitiesContainers.ElementAt(0).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Season { Id = Guid.NewGuid().ToString("N"), Current = true, StartDate = new DateTime(2020,1,1), EndDate = new DateTime(2021, 1, 1), IdCostCenter = entitiesContainers.ElementAt(1).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //               new Specie { Id = Guid.NewGuid().ToString("N"), Name = "Ciruela", Abbreviation = "CI" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Variety { Id = Guid.NewGuid().ToString("N"), Name = "Pink Delight", Abbreviation = "PKD", IdSpecie = entitiesContainers.ElementAt(3).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //       /*5*/   new Variety { Id = Guid.NewGuid().ToString("N"), Name = "Early Queen", Abbreviation = "EARQ", IdSpecie = entitiesContainers.ElementAt(3).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Sector { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new PlotLand { Id = Guid.NewGuid().ToString("N"), Name = "Esmeralda", IdSector = entitiesContainers.ElementAt(6).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Rootstock { Id = Guid.NewGuid().ToString("N"), Name = "Nemaguard", Abbreviation = "NEMG" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Barrack { Id = Guid.NewGuid().ToString("N"), SeasonId = entitiesContainers.ElementAt(2).Entity.Id, Name = "Cuartel X", Hectares = 1.5, NumberOfPlants = 453, PlantingYear = 2000, IdRootstock = entitiesContainers.ElementAt(8).Entity.Id, IdPlotLand = entitiesContainers.ElementAt(7).Entity.Id, IdVariety = entitiesContainers.ElementAt(4).Entity.Id, IdPollinator = entitiesContainers.ElementAt(5).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //       /*10*/  new IngredientCategory { Id = Guid.NewGuid().ToString("N"), Name = "Insecticida" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Ingredient { Id = Guid.NewGuid().ToString("N"), Name = "Lambda-cihalotrina", idCategory = entitiesContainers.ElementAt(10).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Ingredient { Id = Guid.NewGuid().ToString("N"), Name = "Imidacloprid", idCategory = entitiesContainers.ElementAt(10).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new ApplicationTarget { Id = Guid.NewGuid().ToString("N"), Name = "Control de plaga" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new CertifiedEntity { Id = Guid.NewGuid().ToString("N"), Name = "Union Europea", Abbreviation = "UEA" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //       /*15*/  new Product { Id = Guid.NewGuid().ToString("N"), Name = "Geminis Wp", Brand = "Anasac", IdActiveIngredient = entitiesContainers.ElementAt(12).Entity.Id, KindOfBottle = 0, MeasureType = (MeasureType)1, Quantity =  500 } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            ///*Preguntar*/  new Dose { Id = Guid.NewGuid().ToString("N"), IdProduct = entitiesContainers.ElementAt(15).Entity.Id, IdsApplicationTarget = new string[] { entitiesContainers.ElementAt(13).Entity.Id }, IdSpecies = new string[] { entitiesContainers.ElementAt(3).Entity.Id }, IdVarieties = new string[] { entitiesContainers.ElementAt(4).Entity.Id, entitiesContainers.ElementAt(5).Entity.Id }, ApplicationDaysInterval = 15, HoursToReEntryToBarrack = 5, DosesApplicatedTo = (DosesApplicatedTo)1, DosesQuantityMin =  500, DosesQuantityMax = 800, NumberOfSequentialApplication = 3, WaitingDaysLabel = 25, WaitingToHarvest = new List<WaitingHarvest> { new WaitingHarvest { IdCertifiedEntity = entitiesContainers.ElementAt(14).Entity.Id, WaitingDays = 25} }, WettingRecommendedByHectares = 2000 } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new PhenologicalEvent { Id = Guid.NewGuid().ToString("N"), Name = "Aparicion de flor", StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 7, 1) } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new OrderFolder {  Id = Guid.NewGuid().ToString("N"), IdSpecie = entitiesContainers.ElementAt(3).Entity.Id, IdApplicationTarget = entitiesContainers.ElementAt(13).Entity.Id, IdPhenologicalEvent = entitiesContainers.ElementAt(17).Entity.Id, IdIngredientCategory = entitiesContainers.ElementAt(10).Entity.Id, IdIngredient = entitiesContainers.ElementAt(12).Entity.Id } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            ///*Preguntar*/  new PreOrder { Id = Guid.NewGuid().ToString("N"), Name = "Eulia", OrderFolderId = entitiesContainers.ElementAt(18).Entity.Id, IdIngredient = entitiesContainers.ElementAt(12).Entity.Id, PreOrderType = (PreOrderType)1, BarracksId = new string[] { entitiesContainers.ElementAt(9).Entity.Id } } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //       /*20*/  new Tractor { Id = Guid.NewGuid().ToString("N"), Brand = "John Deere", Code = "JDT" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //               new Nebulizer { Id = Guid.NewGuid().ToString("N"), Brand = "Lerpain", Code = "LRP" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Role { Id = Guid.NewGuid().ToString("N"), Name = "Administrador" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Role { Id = Guid.NewGuid().ToString("N"), Name = "Aplicador" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new Job { Id = Guid.NewGuid().ToString("N"), Name = "Administrador" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            //       /*25*/  new User { Id = Guid.NewGuid().ToString("N"), Name = "Cristian Rojas", Email = "cristian.rojas@alumnos.uv.cl", Rut = "19.193.382-6", IdJob = entitiesContainers.ElementAt(24).Entity.Id, IdsRoles = new List<string> { entitiesContainers.ElementAt(22).Entity.Id, entitiesContainers.ElementAt(23).Entity.Id }, ObjectIdAAD = "d273305e-9a05-4bbb-9bfc-ae724610b93a" } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new NotificationEvent { Id = Guid.NewGuid().ToString("N"), Description = "", IdBarrack = entitiesContainers.ElementAt(9).Entity.Id, NotificationType = (NotificationType)1, IdPhenologicalEvent = entitiesContainers.ElementAt(17).Entity.Id, PicturePath = "https://agricolablob.blob.core.windows.net/contenedor013dae60-0e06-43bc-a5a9-f357c4e63de0/8209d00d-6fe0-45dd-b949-801db88b53c1.jpg", Created = DateTime.Today } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = 
            //               new ApplicationOrder { Id = Guid.NewGuid().ToString("N"), Name = "Aplicacion de insecticida en Ciruela", OrderType = (OrderType)1, IdsPreOrder = new string[] { entitiesContainers.ElementAt(19).Entity.Id }, Barracks = new BarrackOrderInstance[] { new BarrackOrderInstance { IdBarrack = entitiesContainers.ElementAt(9).Entity.Id, IdNotificationEvents = new string[] { entitiesContainers.ElementAt(26).Entity.Id } } }, DosesOrder = new DosesOrder[] { new DosesOrder { IdDoses = entitiesContainers.ElementAt(16).Entity.Id, QuantityByHectare = 1350 } }, Wetting = 2000, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 7, 1) } });
            //           entitiesContainers.Add(new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity =
            ///*Preguntar*/  new ExecutionOrder { Id = Guid.NewGuid().ToString("N"), IdOrder = entitiesContainers.ElementAt(27).Entity.Id, IdUserApplicator = entitiesContainers.ElementAt(25).Entity.Id, DosesOrder = new DosesOrder[] { new DosesOrder { IdDoses = entitiesContainers.ElementAt(16).Entity.Id, QuantityByHectare = 1350 } }, IdNebulizer = entitiesContainers.ElementAt(21).Entity.Id, IdTractor = entitiesContainers.ElementAt(20).Entity.Id, StartDate = new DateTime(2020, 5, 10), EndDate = new DateTime(2020, 6, 25) } });

            //           foreach (EntityContainer entityContainer in entitiesContainers)
            //               await repo.CreateUpdate(entityContainer);

            #endregion

            timer.Stop();
            Console.WriteLine("Hora de termino: {0}", DateTime.Now.ToString("hh\\:mm\\:ss"));
            Console.WriteLine("Tiempo transcurrido: {0}", timer.Elapsed.ToString("hh\\:mm\\:ss"));

        }

    }

}