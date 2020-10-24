using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro_model;

namespace trifenix.connect.agro.tests.data
{
    public static class AgroData
    {

        public static Product Product1 => new Product { 
            Id = AgroInputData.Product1.Id,
            IdActiveIngredient = AgroInputData.Product1.IdActiveIngredient,
            IdBrand = AgroInputData.Product1.IdBrand,
            Name = AgroInputData.Product1.Name,
            SagCode = AgroInputData.Product1.SagCode,
            MeasureType = AgroInputData.Product1.MeasureType,
            ClientId = "1"
        };

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

        public static Product[] Products => new Product[] { Product1, Product2 };


        public static Dose Doses1 => new Dose {
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
            LastModified = new DateTime(2020,5,1, 0, 0 ,0)
        };

        public static Dose Dosis2 => new Dose
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


        public static Dose Dosis3 => new Dose
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

        public static Dose Dosis4 => new Dose
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

        public static WaitingHarvest WaitingHarvest1 => new WaitingHarvest {
            IdCertifiedEntity = ConstantGuids.Value[0],
            Ppm = 10,
            WaitingDays = 11
        };

        public static WaitingHarvest WaitingHarvest2 => new WaitingHarvest
        {
            IdCertifiedEntity = ConstantGuids.Value[1],
            Ppm = 10,
            WaitingDays = 11
        };



    }
}
