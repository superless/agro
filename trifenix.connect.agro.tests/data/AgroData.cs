using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro_model;
using trifenix.connect.entities.cosmos;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.tests.mock;
using trifenix.connect.util;

namespace trifenix.connect.agro.tests.data
{

    /// <summary>
    /// Colecciones de elementos de base de datos y entitySearch.
    /// </summary>
    public static class AgroData
    {

        #region products

        /// <summary>
        /// Ejemplo Producto 1
        /// </summary>
        public static Product Product1 => new Product
        {
            Id = AgroInputData.Product1.Id,
            IdActiveIngredient = AgroInputData.Product1.IdActiveIngredient,
            IdBrand = AgroInputData.Product1.IdBrand,
            Name = AgroInputData.Product1.Name,
            SagCode = AgroInputData.Product1.SagCode,
            MeasureType = AgroInputData.Product1.MeasureType,
            ClientId = "1"
        };


        /// <summary>
        /// Ejemplo de producto 2
        /// </summary>
        public static Product Product2 => new Product
        {
            Id = AgroInputData.Product2.Id,
            IdActiveIngredient = AgroInputData.Product2.IdActiveIngredient,
            IdBrand = AgroInputData.Product2.IdBrand,
            Name = AgroInputData.Product2.Name,
            SagCode = AgroInputData.Product2.SagCode,
            MeasureType = AgroInputData.Product2.MeasureType,
            ClientId = "2"
        };

        /// <summary>
        /// Snapshot con los objetos iniciales
        /// </summary>
        public static Product[] ProductsBase => new Product[] { Product1, Product2 };

        /// <summary>
        /// Colección en memoria de productos.
        /// </summary>
        public static Product[] Products { get; set; } = ProductsBase;

        /// <summary>
        /// Snapshot con los entitySearch iniciales.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] ProductSearchBase { get; set; } = ProductsBase.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();


        /// <summary>
        /// Colección en memoria de los entitySearch de productos.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] ProductSearchs { get; set; } = ProductSearchBase;

        #endregion

        /// <summary>
        /// Ejemplo dosis 1
        /// </summary>
        #region dosis
        public static Dose Doses1 => new Dose
        {
            Id = ConstantGuids.Value[0],
            Active = true,
            ApplicationDaysInterval = 10,
            Default = false,
            DosesQuantityMax = 2,
            DosesQuantityMin = 1.1,
            HoursToReEntryToBarrack = 22,
            IdsApplicationTarget = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1],
                            },
            WaitingDaysLabel = 10,
            WettingRecommendedByHectares = 2000,
            NumberOfSequentialApplication = 1,
            IdProduct = ConstantGuids.Value[0],
            // Id = ConstantGuids.Value[1],  // las dosis no deberían tener ids.
            WaitingToHarvest = new List<WaitingHarvest> { WaitingHarvest1, WaitingHarvest2 },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            ClientId = "1",
            LastModified = new DateTime(2020, 5, 1, 0, 0, 0),

        };


        /// <summary>
        /// ejemplo dosis 2
        /// </summary>
        public static Dose Doses2 => new Dose
        {
            Id = ConstantGuids.Value[1],
            Active = true,
            ApplicationDaysInterval = 10,
            Default = false,
            DosesQuantityMax = 2,
            DosesQuantityMin = 1.1,
            HoursToReEntryToBarrack = 22,
            IdsApplicationTarget = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1],
                            },
            WaitingDaysLabel = 10,
            WettingRecommendedByHectares = 2000,
            NumberOfSequentialApplication = 1,
            IdProduct = ConstantGuids.Value[0],
            // Id = ConstantGuids.Value[1],  // las dosis no deberían tener ids.
            WaitingToHarvest = new List<WaitingHarvest> { WaitingHarvest1, WaitingHarvest2 },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },

        };

        /// <summary>
        /// ejemplo dosis3
        /// </summary>
        public static Dose Doses3 => new Dose
        {
            Id = ConstantGuids.Value[2],
            Active = true,
            ApplicationDaysInterval = 10,
            Default = false,
            DosesQuantityMax = 2,
            DosesQuantityMin = 1.1,
            HoursToReEntryToBarrack = 22,
            IdsApplicationTarget = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1],
                            },
            WaitingDaysLabel = 10,
            WettingRecommendedByHectares = 2000,
            NumberOfSequentialApplication = 1,
            IdProduct = ConstantGuids.Value[1],
            // Id = ConstantGuids.Value[1],  // las dosis no deberían tener ids.
            WaitingToHarvest = new List<WaitingHarvest> { WaitingHarvest1, WaitingHarvest2 },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },

        };


        /// <summary>
        /// ejemplo dosis 4
        /// </summary>
        public static Dose Doses4 => new Dose
        {
            Id = ConstantGuids.Value[3],
            Active = true,
            ApplicationDaysInterval = 10,
            Default = false,
            DosesQuantityMax = 2,
            DosesQuantityMin = 1.1,
            HoursToReEntryToBarrack = 22,
            IdsApplicationTarget = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1],
                            },
            WaitingDaysLabel = 10,
            WettingRecommendedByHectares = 2000,
            NumberOfSequentialApplication = 1,
            IdProduct = ConstantGuids.Value[1],
            // Id = ConstantGuids.Value[1],  // las dosis no deberían tener ids.
            WaitingToHarvest = new List<WaitingHarvest> { WaitingHarvest1, WaitingHarvest2 },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },

        };



        /// <summary>
        /// Snapshot con los objetos iniciales
        /// </summary>
        public static Dose[] DosesBase => new Dose[] { Doses1, Doses2, Doses3, Doses4 };


        /// <summary>
        /// Colección en memoria de dosis.
        /// </summary>
        public static Dose[] Doses { get; set; } = DosesBase;

        /// <summary>
        /// Snapshot con los entitySearch iniciales.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] DosesSearchBase { get; set; } = DosesBase.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();


        /// <summary>
        /// Colección en memoria de los entitySearch de dosis.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] DosesSearch { get; set; } = DosesSearchBase;

        #endregion

        #region waiting harvest

        /// <summary>
        /// Waiting Harvest Ejemplo1
        /// </summary>
        public static WaitingHarvest WaitingHarvest1 => new WaitingHarvest
        {
            IdCertifiedEntity = ConstantGuids.Value[0],
            Ppm = 10,
            WaitingDays = 11
        };

        /// <summary>
        /// Waiting Harvest Ejemplo 2
        /// </summary>
        public static WaitingHarvest WaitingHarvest2 => new WaitingHarvest
        {
            IdCertifiedEntity = ConstantGuids.Value[1],
            Ppm = 10,
            WaitingDays = 11
        };

        /// <summary>
        /// Snapshot con los objetos iniciales
        /// </summary>
        public static WaitingHarvest[] WaitingHarvestBase { get; set; } = new WaitingHarvest[] { WaitingHarvest1, WaitingHarvest2 };

        /// <summary>
        /// Colección en memoria de WaitingHarvest.
        /// Waiting harvest no tiene nada que hacer acá, es solo un input sin relación a la base de datos
        /// pero es importante recordarlo.
        /// </summary>
        public static WaitingHarvest[] WaitingHarvest { get; set; } = WaitingHarvestBase;


        /// <summary>
        /// Snapshot con los entitySearch iniciales.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] WaitingHarvestSearchBase { get; set; } = WaitingHarvest.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();

        /// <summary>
        /// Colección en memoria de los entitySearch de waiting harvest.
        /// </summary>
        public static IEntitySearch<GeoPointTs>[] WaitingHarvestSearch { get; set; } = WaitingHarvestSearchBase;
        #endregion

        #region Ingredients
        public static Ingredient Ingredient1 => new Ingredient
        {
            Id = ConstantGuids.Value[0],
            Name = "Ingrediente 1",
            idCategory = ConstantGuids.Value[0]
        };

        public static Ingredient Ingredient2 => new Ingredient
        {
            Id = ConstantGuids.Value[1],
            Name = "Ingrediente 2",
            idCategory = ConstantGuids.Value[1]
        };

        public static Ingredient[] Ingredients { get; set; } = new Ingredient[] { Ingredient1, Ingredient2 };

        public static IEntitySearch<GeoPointTs>[] IngredientsSearch { get; set; } = Ingredients.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();



        #endregion

        #region category ingredient
        public static IngredientCategory CategoryIngredient1 => new IngredientCategory
        {
            Id = ConstantGuids.Value[0],
            Name = "Categoria 1",

        };

        public static IngredientCategory CategoryIngredient2 => new IngredientCategory
        {
            Id = ConstantGuids.Value[1],
            Name = "Categoria 2",

        };

        public static IngredientCategory[] IngredientCategories { get; set; } = new IngredientCategory[] { CategoryIngredient1, CategoryIngredient2 };

        public static IEntitySearch<GeoPointTs>[] IngredientCategoriesSearch { get; set; } = IngredientCategories.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();


        #endregion

        #region brand

        public static Brand Brand1 => new Brand
        {
            Id = ConstantGuids.Value[0],
            Name = "Marca 1"
        };

        public static Brand Brand2 => new Brand
        {
            Id = ConstantGuids.Value[0],
            Name = "Marca 2"
        };

        public static Brand[] Brands { get; set; } = new Brand[] { Brand1, Brand2 };


        public static IEntitySearch<GeoPointTs>[] BrandsSearch { get; set; } = Brands.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();


        #endregion

        #region Specie

        public static Specie Specie1 = new Specie
        {
            Abbreviation = "MZN",
            Name = "Manzana",
            Id = ConstantGuids.Value[0]
        };

        public static Specie Specie2 = new Specie
        {
            Abbreviation = "NCT",
            Name = "Nectarin",
            Id = ConstantGuids.Value[1]
        };

        public static Specie[] Species { get; set; } = new Specie[] { Specie1, Specie2 };


        public static IEntitySearch<GeoPointTs>[] SpeciesSearch { get; set; } = Species.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();



        #endregion

        #region Variety

        public static Variety Variety1 = new Variety
        {
            Abbreviation = "FJ",
            Name = "Fuji",
            IdSpecie = ConstantGuids.Value[0],
            Id = ConstantGuids.Value[0]
        };

        public static Variety Variety2 = new Variety
        {
            Abbreviation = "GLD",
            Name = "GOLDEN",
            IdSpecie = ConstantGuids.Value[0],
            Id = ConstantGuids.Value[1]
        };

        public static Variety Variety3 = new Variety
        {
            Abbreviation = "AML",
            Name = "Amarillo",
            IdSpecie = ConstantGuids.Value[1],
            Id = ConstantGuids.Value[2]
        };

        public static Variety Variety4 = new Variety
        {
            Abbreviation = "BLK",
            Name = "Blanco",
            IdSpecie = ConstantGuids.Value[1],
            Id = ConstantGuids.Value[3]
        };

        public static Variety[] Varieties { get; set; } = new Variety[] { Variety1, Variety2, Variety3, Variety4 };


        public static IEntitySearch<GeoPointTs>[] VarietiesSearch { get; set; } = Varieties.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();

        #endregion


        #region ApplicationTarget
        public static ApplicationTarget ApplicationTarget1 = new ApplicationTarget
        {
            Abbreviation = "ENF1",
            Name = "Enfermedad 1",
            Id = ConstantGuids.Value[0]
        };

        public static ApplicationTarget ApplicationTarget2 = new ApplicationTarget
        {
            Abbreviation = "ENF2",
            Name = "Enfermedad 2",
            Id = ConstantGuids.Value[1]
        };

        public static ApplicationTarget[] ApplicationTargets { get; set; } = new ApplicationTarget[] { ApplicationTarget1, ApplicationTarget2 };


        public static IEntitySearch<GeoPointTs>[] ApplicationTargetsSearch { get; set; } = ApplicationTargets.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();



        #endregion

        #region certifiedEntity
        public static CertifiedEntity CertifiedEntity1 = new CertifiedEntity
        {
            Abbreviation = "USA",
            Name = "Estados Unidos",
            Id = ConstantGuids.Value[0]
        };

        public static CertifiedEntity CertifiedEntity2 = new CertifiedEntity
        {
            Abbreviation = "CHN",
            Name = "China",
            Id = ConstantGuids.Value[1]
        };

        public static CertifiedEntity[] CertifiedEntities { get; set; } = new CertifiedEntity[] { CertifiedEntity1, CertifiedEntity2 };

        public static IEntitySearch<GeoPointTs>[] CertifiedEntitiesSearch { get; set; } = CertifiedEntities.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion


        #region Barrack
        public static Barrack Barrack1 => new Barrack
        {
            Id = ConstantGuids.Value[0],
            Hectares = 1.1,
            IdPlotLand = ConstantGuids.Value[0],
            IdPollinator = ConstantGuids.Value[0],
            NumberOfPlants = 444,
            IdVariety = ConstantGuids.Value[0],
            Name = "Barrack 1",
            SeasonId = ConstantGuids.Value[0],
            IdRootstock = ConstantGuids.Value[0],
            PlantingYear = 1980,

        };

        public static Barrack Barrack2 => new Barrack
        {
            Id = ConstantGuids.Value[1],
            Hectares = 1.1,
            IdPlotLand = ConstantGuids.Value[1],
            IdPollinator = ConstantGuids.Value[1],
            NumberOfPlants = 444,
            IdVariety = ConstantGuids.Value[1],
            Name = "Barrack 2",
            SeasonId = ConstantGuids.Value[0],
            IdRootstock = ConstantGuids.Value[1],
            PlantingYear = 1980,
        };


        public static Barrack[] Barracks { get; set; } = new Barrack[] { Barrack1, Barrack2 };

        public static IEntitySearch<GeoPointTs>[] BarracksSearch { get; set; } = Barracks.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();

        #endregion

        #region Plotland
        public static PlotLand Plotland1 => new PlotLand
        {

            Id = ConstantGuids.Value[0],
            IdSector = ConstantGuids.Value[0],
            Name = "Parcela 1"

        };

        public static PlotLand Plotland2 => new PlotLand
        {

            Id = ConstantGuids.Value[1],
            IdSector = ConstantGuids.Value[0],
            Name = "Parcela 2"
        };

        public static PlotLand Plotland3 => new PlotLand
        {
            Id = ConstantGuids.Value[2],
            IdSector = ConstantGuids.Value[1],
            Name = "Parcela 3"
        };

        public static PlotLand Plotland4 => new PlotLand
        {
            Id = ConstantGuids.Value[3],
            IdSector = ConstantGuids.Value[1],
            Name = "Parcela 4"
        };


        public static PlotLand[] PlotLands { get; set; } = new PlotLand[] { Plotland1, Plotland2, Plotland3, Plotland4 };

        public static IEntitySearch<GeoPointTs>[] PlotLandsSearch { get; set; } = PlotLands.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region Sector
        public static Sector Sector1 => new Sector
        {
            Id = ConstantGuids.Value[0],
            Name = "Sector 1"
        };

        public static Sector Sector2 => new Sector
        {
            Id = ConstantGuids.Value[1],
            Name = "Sector 2"
        };

        public static Sector[] Sectors { get; set; } = new Sector[] { Sector1, Sector2 };


        public static IEntitySearch<GeoPointTs>[] SectorsSearch { get; set; } = Sectors.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();



        #endregion


        #region Season
        public static Season Season1 => new Season
        {
            Id = ConstantGuids.Value[0],
            Current = true,
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdCostCenter = ConstantGuids.Value[0]
        };

        public static Season Season2 => new Season
        {
            Id = ConstantGuids.Value[1],
            Current = false,
            StartDate = new DateTime(2020, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdCostCenter = ConstantGuids.Value[1]
        };

        public static Season[] Seasons { get; set; } = new Season[] { Season1, Season2 };


        public static IEntitySearch<GeoPointTs>[] SeasonsSearch { get; set; } = Seasons.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();


        #endregion

        #region Rootstock
        public static Rootstock Rootstock1 => new Rootstock
        {
            Id = ConstantGuids.Value[0],
            Name = "Rootstock 1",
            Abbreviation = "RT1"
        };

        public static Rootstock Rootstock2 => new Rootstock
        {
            Id = ConstantGuids.Value[1],
            Name = "Rootstock 2",
            Abbreviation = "RT2"
        };

        public static Rootstock[] Rootstocks { get; set; } = new Rootstock[] {
          Rootstock1,
          Rootstock2
        };

        public static IEntitySearch<GeoPointTs>[] RootstocksSearch { get; set; } = Rootstocks.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();

        #endregion

        #region CostCenter

        public static CostCenter CostCenter1 => new CostCenter
        {
            Id = ConstantGuids.Value[0],
            IdBusinessName = ConstantGuids.Value[0],
            Name = "CostCenter 1"
        };

        public static CostCenter CostCenter2 => new CostCenter
        {
            Id = ConstantGuids.Value[1],
            IdBusinessName = ConstantGuids.Value[1],
            Name = "CostCenter 2"
        };

        public static CostCenter[] CostCenters { get; set; } = new CostCenter[] {
            CostCenter1,
            CostCenter2
        };

        public static IEntitySearch<GeoPointTs>[] CostCentersSearch { get; set; } = CostCenters.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion


        #region BusinessName
        public static BusinessName BusinessNameInput1 => new BusinessName
        {
            Email = "hola@trifenix.com",
            Giro = "Tecnología",
            Id = ConstantGuids.Value[0],
            Name = "Trifenix",
            Phone = "999999999",
            Rut = "76965261-2",
            WebPage = "www.trifenix.io"
        };

        public static BusinessName BusinessNameInput2 => new BusinessName
        {
            Email = "hola@tricomar.cl",
            Giro = "e-commerce",
            Id = ConstantGuids.Value[1],
            Name = "Tricomar",
            Phone = "8888888",
            Rut = "5760300-3",
            WebPage = "www.tricomar.cl"
        };

        public static BusinessName[] BusinessNames { get; set; } = new BusinessName[] { BusinessNameInput1, BusinessNameInput2 };

        public static IEntitySearch<GeoPointTs>[] BusinessNamesSearch { get; set; } = BusinessNames.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();

        #endregion

        #region Job
        public static Job JobInput1 => new Job
        {
            Name = "Trabajo1",
            Id = ConstantGuids.Value[0]
        };

        public static Job JobInput2 => new Job
        {
            Name = "Trabajo2",
            Id = ConstantGuids.Value[1]
        };

        public static Job[] Jobs { get; set; } = new Job[] { JobInput1, JobInput2 };

        public static IEntitySearch<GeoPointTs>[] JobsSearch { get; set; } = Jobs.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region Nebulizer

        public static Nebulizer NebulizerInput1 => new Nebulizer
        {
            Brand = "Marca1",
            Id = ConstantGuids.Value[0],
            Code = "Codigo1"
        };
        public static Nebulizer NebulizerInput2 => new Nebulizer
        {
            Brand = "Marca2",
            Id = ConstantGuids.Value[1],
            Code = "Codigo2"
        };

        public static Nebulizer[] Nebulizers { get; set; } = new Nebulizer[] { NebulizerInput1, NebulizerInput2};

        public static IEntitySearch<GeoPointTs>[] NebulizersSearch { get; set; } = Nebulizers.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region PhenologicalEvent

        public static PhenologicalEvent PhenologicalEventInput1 => new PhenologicalEvent
        {
            Name = "Evento fenologico 1",
            Id = ConstantGuids.Value[0],
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1)
        };
        public static PhenologicalEvent PhenologicalEventInput2 => new PhenologicalEvent
        {
            Name = "Evento fenologico 2",
            Id = ConstantGuids.Value[1],
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1)
        };

        public static PhenologicalEvent[] PhenologicalEvents { get; set; } = new PhenologicalEvent[] { PhenologicalEventInput1, PhenologicalEventInput2 };

        public static IEntitySearch<GeoPointTs>[] PhenologicalEventsSearch { get; set; } = PhenologicalEvents.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region Role

        public static Role RoleInput1 => new Role
        {
            Name = "Rol 1",
            Id = ConstantGuids.Value[0],

        };
        public static Role RoleInput2 => new Role
        {
            Name = "Rol 2",
            Id = ConstantGuids.Value[1],

        };

        public static Role[] Roles { get; set; } = new Role[] { RoleInput1, RoleInput2 };

        public static IEntitySearch<GeoPointTs>[] RolesSearch { get; set; } = Roles.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region PreOrden

        public static PreOrder PreOrdenInput1 => new PreOrder
        {
            Name = "Pre orden 1",
            Id = ConstantGuids.Value[0],
            IdIngredient = ConstantGuids.Value[0],
            OrderFolderId = ConstantGuids.Value[0],
            PreOrderType = PreOrderType.DEFAULT,
            BarracksId = ConstantGuids.Value 
        };
        public static PreOrder PreOrdenInput2 => new PreOrder
        {
            Name = "Pre orden 2",
            Id = ConstantGuids.Value[1],
            IdIngredient = ConstantGuids.Value[1],
            OrderFolderId = ConstantGuids.Value[1],
            PreOrderType = PreOrderType.PHENOLOGICAL,
            BarracksId = ConstantGuids.Value

        };

        public static PreOrder[] PreOrders { get; set; } = new PreOrder[] { PreOrdenInput1, PreOrdenInput2 };

        public static IEntitySearch<GeoPointTs>[] PreOrdersSearch { get; set; } = PreOrders.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region Tractor

        public static Tractor TractorInput1 => new Tractor
        {
            Id = ConstantGuids.Value[0],
            Brand = ConstantGuids.Value[0],
            Code = ConstantGuids.Value[0]
            
        };
        public static Tractor TractorInput2 => new Tractor
        {
            Id = ConstantGuids.Value[1],
            Brand = ConstantGuids.Value[1],
            Code = ConstantGuids.Value[1]

        };

        public static Tractor[] Tractors { get; set; } = new Tractor[] { TractorInput1, TractorInput2 };

        public static IEntitySearch<GeoPointTs>[] TractorsSearch { get; set; } = Tractors.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region UserApplicator

        public static UserApplicator UserApplicatorInput1 => new UserApplicator
        {
            Id = ConstantGuids.Value[0],
            IdNebulizer = ConstantGuids.Value[0],
            IdTractor = ConstantGuids.Value[0],
            ObjectIdAAD = ConstantGuids.Value[0],
            Name = "Trifenix",
            Email = "Trifenix@trifenix.io",
            Rut = "20200200-5",
            IdJob = ConstantGuids.Value[0],
            IdsRoles = new List<string> { RoleInput1.Id, RoleInput2.Id }
        };
        public static UserApplicator UserApplicatorInput2 => new UserApplicator
        {
            Id = ConstantGuids.Value[1],
            IdNebulizer = ConstantGuids.Value[1],
            IdTractor = ConstantGuids.Value[1],
            ObjectIdAAD = ConstantGuids.Value[1],
            Name = "Trifenix2",
            Email = "Trifenix2@trifenix.io",
            Rut = "20100100-9",
            IdJob = ConstantGuids.Value[1],
            IdsRoles = new List<string> { RoleInput1.Id, RoleInput2.Id }
        };

        public static UserApplicator[] UserApplicators { get; set; } = new UserApplicator[] { UserApplicatorInput1, UserApplicatorInput2 };

        public static IEntitySearch<GeoPointTs>[] UserApplicatorsSearch { get; set; } = UserApplicators.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion


        #region NotificationEvent

        public static NotificationEvent NotificationEventInput1 => new NotificationEvent
        {
            Id = ConstantGuids.Value[0],
            IdBarrack = ConstantGuids.Value[0],
            IdPhenologicalEvent = ConstantGuids.Value[0],
            NotificationType = NotificationType.Default,
            PicturePath = ConstantGuids.Value[0],
            Description = ConstantGuids.Value[0],
            Created = new DateTime(2021, 3, 1),
            Weather = new Weather()
        };
        public static NotificationEvent NotificationEventInput2 => new NotificationEvent
        {
            Id = ConstantGuids.Value[1],
            IdBarrack = ConstantGuids.Value[1],
            IdPhenologicalEvent = ConstantGuids.Value[1],
            NotificationType = NotificationType.Phenological,
            PicturePath = ConstantGuids.Value[1],
            Description = ConstantGuids.Value[1],
            Created = new DateTime(2021, 3, 1),
            Weather = new Weather()
        };

        public static NotificationEvent[] NotificationEvents { get; set; } = new NotificationEvent[] { NotificationEventInput1, NotificationEventInput2 };

        public static IEntitySearch<GeoPointTs>[] NotificationEventsSearch { get; set; } = NotificationEvents.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region OrderFolder

        public static OrderFolder OrdenFolderInput1 => new OrderFolder
        {
            Id = ConstantGuids.Value[0],
            IdPhenologicalEvent = ConstantGuids.Value[0],
            IdApplicationTarget = ConstantGuids.Value[0],
            IdSpecie = ConstantGuids.Value[0],
            IdIngredient = ConstantGuids.Value[0],
            IdIngredientCategory = ConstantGuids.Value[0]
        };
        public static OrderFolder OrdenFolderInput2 => new OrderFolder
        {
            Id = ConstantGuids.Value[1],
            IdPhenologicalEvent = ConstantGuids.Value[1],
            IdApplicationTarget = ConstantGuids.Value[1],
            IdSpecie = ConstantGuids.Value[1],
            IdIngredient = ConstantGuids.Value[1],
            IdIngredientCategory = ConstantGuids.Value[1]
        };

        public static OrderFolder[] OrderFolders { get; set; } = new OrderFolder[] { OrdenFolderInput1, OrdenFolderInput2 };

        public static IEntitySearch<GeoPointTs>[] OrderFoldersSearch { get; set; } = OrderFolders.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion

        #region ExecutionOrder
        //terminar
        public static ExecutionOrder ExecutionOrderInput1 => new ExecutionOrder
        {
            Id = ConstantGuids.Value[0],
            IdOrder = ConstantGuids.Value[0],
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdUserApplicator = ConstantGuids.Value[0],
            IdNebulizer = ConstantGuids.Value[0],
            IdTractor = ConstantGuids.Value[0],
            //DosesOrder
        };
        public static ExecutionOrder ExecutionOrderInput2 => new ExecutionOrder
        {
            Id = ConstantGuids.Value[1],
            IdOrder = ConstantGuids.Value[1],
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdUserApplicator = ConstantGuids.Value[0],
            IdNebulizer = ConstantGuids.Value[0],
            IdTractor = ConstantGuids.Value[0],
        };

        public static ExecutionOrder[] ExecutionOrders { get; set; } = new ExecutionOrder[] { ExecutionOrderInput1, ExecutionOrderInput2 };

        public static IEntitySearch<GeoPointTs>[] ExecutionOrdersSearch { get; set; } = ExecutionOrders.SelectMany(s => Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToArray();
        #endregion
    }
}
