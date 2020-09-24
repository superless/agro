using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.agro.index_model.enums;
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
                                ConstantGuids.Value[0]
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
            SagCode = "43321",
            Doses = new DosesInput[] { Doses1, Doses2 }

        };


        public static ProductInput[] Products => new ProductInput[] { Product1, Product2 };
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


    #endregion

}







}
