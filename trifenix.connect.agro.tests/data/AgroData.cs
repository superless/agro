using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.external.operations.tests.data
{

    public static class AgroData {

        #region Doses
        public static DosesInput Doses1 => new DosesInput
        {
            Active = true,
            ApplicationDaysInterval = 10,
            DosesApplicatedTo = DosesApplicatedTo.L100,
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
            WaitingToHarvest = new WaitingHarvestInput[] { WaitingHarvest1, WaitingHarvest2 },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },

        };

        public static DosesInput Doses2 => new DosesInput
        {
            Active = true,
            ApplicationDaysInterval = 10,
            DosesApplicatedTo = DosesApplicatedTo.L1000,
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
            WaitingToHarvest = new WaitingHarvestInput[]{WaitingHarvest1, WaitingHarvest2,
                            },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[0]
                            },

        };


        public static DosesInput DosesWithoutWettingRecommended => new DosesInput
        {
            Active = true,
            ApplicationDaysInterval = 10,
            DosesApplicatedTo = DosesApplicatedTo.L1000,
            Default = false,
            DosesQuantityMax = 2,
            DosesQuantityMin = 1.1,
            HoursToReEntryToBarrack = 22,
            IdsApplicationTarget = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1],
                            },
            WaitingDaysLabel = 10,
            NumberOfSequentialApplication = 1,
            IdProduct = ConstantGuids.Value[0],
            // Id = ConstantGuids.Value[1],  // las dosis no deberían tener ids.
            WaitingToHarvest = new WaitingHarvestInput[]{WaitingHarvest1, WaitingHarvest2,
                            },
            IdSpecies = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[1]
                            },
            IdVarieties = new string[]{
                                ConstantGuids.Value[0],
                                ConstantGuids.Value[0]
                            },

        };
        #endregion

        #region waiting harvest
        public static WaitingHarvestInput WaitingHarvest1 => new WaitingHarvestInput
        {
            IdCertifiedEntity = ConstantGuids.Value[0],
            Ppm = 10,
            WaitingDays = 11
        };

        public static WaitingHarvestInput WaitingHarvest2 => new WaitingHarvestInput
        {
            IdCertifiedEntity = ConstantGuids.Value[0],
            Ppm = 10,
            WaitingDays = 11
        };
        #endregion

        #region Products
        public static ProductInput Product1 => new ProductInput
        {
            Id = ConstantGuids.Value[0],
            IdBrand = ConstantGuids.Value[0],
            IdActiveIngredient = ConstantGuids.Value[0],
            Name = "Producto 1",
            MeasureType = MeasureType.KL,
            SagCode = "12234",
            Doses = new DosesInput[]{
                        Doses1, Doses2
                    }

        };





        public static ProductInput Product2 => new ProductInput
        {
            Id = ConstantGuids.Value[1],
            IdBrand = ConstantGuids.Value[1],
            IdActiveIngredient = ConstantGuids.Value[1],
            Name = "Producto 2",
            MeasureType = MeasureType.LT,
            SagCode = "43322",
            Doses = new DosesInput[] { Doses1, Doses2 }

        };



        public static ProductInput ProductWithDosesWithoutWett => new ProductInput
        {
            Id = ConstantGuids.Value[0],
            IdBrand = ConstantGuids.Value[0],
            IdActiveIngredient = ConstantGuids.Value[0],
            Name = "Producto 1",
            MeasureType = MeasureType.KL,
            SagCode = "12234",
            Doses = new DosesInput[]{
                        DosesWithoutWettingRecommended, Doses2
                    }

        };

        public static ProductInput[] Products => new ProductInput[] { Product1, Product2 };

        public static ProductInput ProductNewWithoutBrand => new ProductInput
        {
            IdBrand = string.Empty,

            IdActiveIngredient = ConstantGuids.Value[1],
            Name = "Producto 3",
            MeasureType = MeasureType.LT,
            SagCode = "43323"

        };

        public static ProductInput ProductWithInvalidIdToEdit => new ProductInput
        {
            Id = Guid.NewGuid().ToString("N"),
            IdBrand = ConstantGuids.Value[1],
            IdActiveIngredient = ConstantGuids.Value[1],
            Name = "Producto 4",
            MeasureType = MeasureType.LT,
            SagCode = "43323",
            Doses = new DosesInput[] { Doses1, Doses2 }

        };

        #endregion


        #region Ingredients
        public static IngredientInput Ingredient1 => new IngredientInput
        {
            Id = ConstantGuids.Value[0],
            Name = "Ingrediente 1",
            idCategory = ConstantGuids.Value[0]
        };

        public static IngredientInput Ingredient2 => new IngredientInput
        {
            Id = ConstantGuids.Value[1],
            Name = "Ingrediente 2",
            idCategory = ConstantGuids.Value[1]
        };

        public static IngredientInput[] Ingredients => new IngredientInput[] { Ingredient1, Ingredient2 };
        #endregion


        #region Category Ingredient
        public static IngredientCategoryInput CategoryIngredient1 => new IngredientCategoryInput {
            Id = ConstantGuids.Value[0],
            Name = "Categoria 1",

        };

        public static IngredientCategoryInput CategoryIngredient2 => new IngredientCategoryInput
        {
            Id = ConstantGuids.Value[1],
            Name = "Categoria 2",

        };

        public static IngredientCategoryInput[] IngredientCategories => new IngredientCategoryInput[] { CategoryIngredient1, CategoryIngredient2 };

        public static BrandInput Brand1 => new BrandInput
        {
            Id = ConstantGuids.Value[0],
            Name = "Marca 1"
        };


        public static BrandInput Brand2 => new BrandInput
        {
            Id = ConstantGuids.Value[0],
            Name = "Marca 2"
        };

        public static BrandInput[] Brands => new BrandInput[] { Brand1, Brand2 };


        #endregion

        #region Specie

        public static SpecieInput Specie1 = new SpecieInput
        {
            Abbreviation = "MZN",
            Name = "Manzana",
            Id = ConstantGuids.Value[0]
        };

        public static SpecieInput Specie2 = new SpecieInput
        {
            Abbreviation = "NCT",
            Name = "Nectarin",
            Id = ConstantGuids.Value[1]
        };

        public static SpecieInput[] Species => new SpecieInput[] { Specie1, Specie2 };



        #endregion


        #region Variety

        public static VarietyInput Variety1 = new VarietyInput
        {
            Abbreviation = "FJ",
            Name = "Fuji",
            IdSpecie = ConstantGuids.Value[0],
            Id = ConstantGuids.Value[0]
        };

        public static VarietyInput Variety2 = new VarietyInput
        {
            Abbreviation = "GLD",
            Name = "GOLDEN",
            IdSpecie = ConstantGuids.Value[0],
            Id = ConstantGuids.Value[1]
        };

        public static VarietyInput Variety3 = new VarietyInput
        {
            Abbreviation = "AML",
            Name = "Amarillo",
            IdSpecie = ConstantGuids.Value[1],
            Id = ConstantGuids.Value[2]
        };

        public static VarietyInput Variety4 = new VarietyInput
        {
            Abbreviation = "BLK",
            Name = "Blanco",
            IdSpecie = ConstantGuids.Value[1],
            Id = ConstantGuids.Value[3]
        };

        public static VarietyInput[] Varieties => new VarietyInput[] { Variety1, Variety2, Variety3, Variety4 };

        #endregion

        #region ApplicationTarget
        public static ApplicationTargetInput ApplicationTarget1 = new ApplicationTargetInput
        {
            Abbreviation = "ENF1",
            Name = "Enfermedad 1",
            Id = ConstantGuids.Value[0]
        };

        public static ApplicationTargetInput ApplicationTarget2 = new ApplicationTargetInput
        {
            Abbreviation = "ENF2",
            Name = "Enfermedad 2",
            Id = ConstantGuids.Value[1]
        };

        public static ApplicationTargetInput[] ApplicationTargets => new ApplicationTargetInput[] { ApplicationTarget1, ApplicationTarget2 };

        #endregion

        #region certifiedEntity
        public static CertifiedEntityInput CertifiedEntity1 = new CertifiedEntityInput
        {
            Abbreviation = "USA",
            Name = "Estados Unidos",
            Id = ConstantGuids.Value[0]
        };

        public static CertifiedEntityInput CertifiedEntity2 = new CertifiedEntityInput
        {
            Abbreviation = "CHN",
            Name = "China",
            Id = ConstantGuids.Value[1]
        };

        public static CertifiedEntityInput[] CertifiedEntities => new CertifiedEntityInput[] { CertifiedEntity1, CertifiedEntity2 };

        #endregion


        #region Barrack
        public static BarrackInput Barrack1 => new BarrackInput {
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

        public static BarrackInput Barrack2 => new BarrackInput
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


        public static BarrackInput[] Barracks => new BarrackInput[] { Barrack1, Barrack2 };
        #endregion

        #region Plotland
        public static PlotLandInput Plotland1 => new PlotLandInput
        {

            Id = ConstantGuids.Value[0],
            IdSector = ConstantGuids.Value[0],
            Name = "Parcela 1"

        };

        public static PlotLandInput Plotland2 => new PlotLandInput
        {

            Id = ConstantGuids.Value[1],
            IdSector = ConstantGuids.Value[0],
            Name = "Parcela 2"
        };

        public static PlotLandInput Plotland3 => new PlotLandInput
        {
            Id = ConstantGuids.Value[2],
            IdSector = ConstantGuids.Value[1],
            Name = "Parcela 3"
        };

        public static PlotLandInput Plotland4 => new PlotLandInput
        {
            Id = ConstantGuids.Value[3],
            IdSector = ConstantGuids.Value[1],
            Name = "Parcela 4"
        };


        public static PlotLandInput[] PlotLands => new PlotLandInput[] { Plotland1, Plotland2, Plotland3, Plotland4 };

        #endregion

        #region Sector
        public static SectorInput Sector1 => new SectorInput {
            Id = ConstantGuids.Value[0],
            Name = "Sector 1"
        };

        public static SectorInput Sector2 => new SectorInput
        {
            Id = ConstantGuids.Value[1],
            Name = "Sector 2"
        };

        public static SectorInput[] Sectors => new SectorInput[] { Sector1, Sector2 };



        #endregion


        #region Season
        public static SeasonInput Season1 => new SeasonInput {
            Id = ConstantGuids.Value[0],
            Current = true,
            StartDate = new DateTime(2021, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdCostCenter = ConstantGuids.Value[0]
        };

        public static SeasonInput Season2 => new SeasonInput
        {
            Id = ConstantGuids.Value[1],
            Current = false,
            StartDate = new DateTime(2020, 3, 1),
            EndDate = new DateTime(2022, 3, 1),
            IdCostCenter = ConstantGuids.Value[1]
        };

        public static SeasonInput[] Seasons => new SeasonInput[] { Season1, Season2 };



        #endregion

        #region Rootstock
        public static RootstockInput Rootstock1 => new RootstockInput { 
            Id = ConstantGuids.Value[0],
            Name = "Rootstock 1",
            Abbreviation = "RT1"
        };

        public static RootstockInput Rootstock2 => new RootstockInput
        {
            Id = ConstantGuids.Value[1],
            Name = "Rootstock 2",
            Abbreviation="RT2"
        };

        public static RootstockInput[] Rootstocks => new RootstockInput[] { 
          Rootstock1,
          Rootstock2
        };

        #endregion

        #region CostCenter

        public static CostCenterInput CostCenter1 => new CostCenterInput { 
            Id = ConstantGuids.Value[0],
            IdBusinessName = ConstantGuids.Value[0],
            Name = "CostCenter 1"
        };

        public static CostCenterInput CostCenter2 => new CostCenterInput
        {
            Id = ConstantGuids.Value[1],
            IdBusinessName = ConstantGuids.Value[1],
            Name = "CostCenter 2"
        };

        public static CostCenterInput[] CostCenters => new CostCenterInput[] { 
            CostCenter1, 
            CostCenter2
        };


        #endregion


        #region BusinessName
        public static BusinessNameInput BusinessNameInput1 => new BusinessNameInput { 
            Email="hola@trifenix.com",
            Giro="Tecnología",
            Id = ConstantGuids.Value[0],
            Name = "Trifenix",
            Phone = "999999999",
            Rut = "76965261-2",
            WebPage = "www.trifenix.io"
        };

        public static BusinessNameInput BusinessNameInput2 => new BusinessNameInput
        {
            Email = "hola@tricomar.cl",
            Giro = "e-commerce",
            Id = ConstantGuids.Value[1],
            Name = "Tricomar",
            Phone = "8888888",
            Rut = "5760300-3",
            WebPage = "www.tricomar.cl"
        };

        public static BusinessNameInput[] BusinessNames => new BusinessNameInput[] { BusinessNameInput1, BusinessNameInput2 };





        #endregion
    }







}
