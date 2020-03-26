﻿using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.external.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.operations;

namespace trifenix.agro.console
{
    public static class SearchHelper
    {
        public static async Task UpdateSearch()
        {
            var dbArguments = new AgroDbArguments
            {
                EndPointUrl = "https://agricola-jhm.documents.azure.com:443/",
                NameDb = "agrodb",
                PrimaryKey = "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw=="
            };

            var searchServiceInstance = new AgroSearch("agrosearch", "016DAA5EF1158FEEEE58DA60996D5981");



            

            var agroManager = new AgroManager(dbArguments, null, null, null, searchServiceInstance, "ba7e86c8-6c2d-491d-bb2e-0dd39fdf5dc1");


            var rootStocks = await agroManager.Rootstock.GetElements();

            var roles = await agroManager.Role.GetElements();

            var seasons = await agroManager.Season.GetElements();

            var costCenter = await agroManager.CostCenter.GetElements();



            var species = await agroManager.Specie.GetElements(); // search

            var varieties = await agroManager.Variety.GetElements(); // search

            var barracks = await agroManager.Barrack.GetElements(); // search

            var businessNames = await agroManager.BusinessName.GetElements(); // search

            
            var sectors = await agroManager.Sector.GetElements();
            var certifiedEntity = await agroManager.CertifiedEntity.GetElements(); // seach

            var plotlands = await agroManager.PlotLand.GetElements();
            //var users = await agroManager.UserApplicator.GetElements(); // search
            var jobs = await agroManager.Job.GetElements(); // search
            var ingredients = await agroManager.Ingredient.GetElements();

            var targets = await agroManager.ApplicationTarget.GetElements();

            var products = await agroManager.Product.GetElements();

            var doses = await agroManager.Doses.GetElements();

            if (products.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in products.Result)
                {
                    var dosesProduct = new List<Doses>();
                    var dosesProductId = doses.Result.Where(s => s.IdProduct.Equals(item.Id) && !s.Default).ToList();
                    if (dosesProductId.Any())
                    {
                        dosesProduct.AddRange(dosesProductId);
                    }

                    await agroManager.Product.Save(new ProductInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Brand = item.Brand,
                        IdActiveIngredient = item.IdActiveIngredient,
                        KindOfBottle = item.KindOfBottle,
                        MeasureType = item.MeasureType,
                        Quantity = item.Quantity,
                        Doses = dosesProductId.Select(s=>new DosesInput { 
                            Active = s.Active,
                            ApplicationDaysInterval = s.ApplicationDaysInterval,
                            Default = s.Default,
                            DosesApplicatedTo = s.DosesApplicatedTo,
                            DosesQuantityMax = s.DosesQuantityMax,
                            DosesQuantityMin = s.DosesQuantityMin,
                            HoursToReEntryToBarrack = s.HoursToReEntryToBarrack,
                            IdProduct = s.IdProduct,
                            idsApplicationTarget = s.IdsApplicationTarget,
                            IdSpecies = s.IdSpecies,
                            IdVarieties = s.IdVarieties,
                            NumberOfSequentialApplication = s.NumberOfSequentialApplication,
                            WaitingDaysLabel = s.WaitingDaysLabel,
                            WaitingToHarvest = s.WaitingToHarvest.Any()?s.WaitingToHarvest.Select(a=>new WaitingHarvestInput { IdCertifiedEntity = a.IdCertifiedEntity, Ppm = a.Ppm, WaitingDays = a.WaitingDays }).ToArray():null,
                            WettingRecommendedByHectares = s.WettingRecommendedByHectares,
                            
                        }).ToArray()
                    });
                }
            }


            if (targets.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in targets.Result)
                {
                    await agroManager.ApplicationTarget.Save(new TargetInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Abbreviation
                    });
                }
            }



            if (ingredients.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in ingredients.Result)
                {
                    await agroManager.Ingredient.Save(new IngredientInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        idCategory = item.idCategory


                    });
                }
            }


            if (jobs.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in jobs.Result)
                {
                    await agroManager.Job.Save(new JobInput
                    {
                        Id = item.Id,
                        Name = item.Name
                        

                    });
                }
            }

            if (costCenter.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in costCenter.Result)
                {
                    await agroManager.CostCenter.Save(new CostCenterInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IdBusinessName = item.IdBusinessName
                        
                    });
                }
            }

            //if (users.StatusResult == ExtGetDataResult.Success)
            //{
            //    foreach (var item in users.Result)
            //    {
            //        await agroManager.UserApplicator.Save(new UserApplicatorInput
            //        {
            //            Id = item.Id,
            //            Name = item.Name,
            //            Email = item.Email,
            //            IdJob = item.IdJob,
            //            IdNebulizer = item.IdNebulizer,
            //            IdsRoles = item.IdsRoles,
            //            IdTractor = item.IdTractor,
            //            Rut = item.Rut
            //        });
            //    }
            //}


            if (roles.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in roles.Result)
                {
                    await agroManager.Role.Save(new RoleInput
                    {
                        Id = item.Id,
                        Name = item.Name

                    });
                }
            }


            if (plotlands.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in plotlands.Result)
                {
                    await agroManager.PlotLand.Save(new PlotLandInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IdSector = item.IdSector


                    });
                }
            }



            if (certifiedEntity.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in certifiedEntity.Result)
                {
                    await agroManager.CertifiedEntity.Save(new CertifiedEntityInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Abbreviation

                    });
                }
            }

            if (rootStocks.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in rootStocks.Result)
                {
                    await agroManager.Rootstock.Save(new RootstockInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Abbreviation
                    });
                }
            }

            if (sectors.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in sectors.Result)
                {
                    await agroManager.Sector.Save(new SectorInput
                    {
                        Id = item.Id,
                        Name = item.Name

                    });
                }
            }


            if (seasons.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in seasons.Result)
                {
                    await agroManager.Season.Save(new SeasonInput
                    {
                        Id = item.Id,
                        Current = item.Current,
                        EndDate = item.EndDate,
                        StartDate = item.StartDate,
                        IdCostCenter = item.IdCostCenter

                    });
                }
            }


            if (businessNames.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in businessNames.Result)
                {
                    await agroManager.BusinessName.Save(new BusinessNameInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Email = item.Email,
                        Giro = item.Giro,
                        Phone = item.Phone,
                        Rut = item.Rut,
                        WebPage = item.WebPage

                    });
                }
            }

            if (species.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in species.Result)
                {
                    await agroManager.Specie.Save(new SpecieInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Name
                    });
                }
            }

            if (varieties.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in varieties.Result)
                {
                    await agroManager.Variety.Save(new VarietyInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Name,
                        IdSpecie = item.IdSpecie
                    });
                }
            }

            if (barracks.StatusResult == ExtGetDataResult.Success)
            {
                
                foreach (var item in barracks.Result)
                {
                    var geoPoints = 
                        item.GeographicalPoints.Any() ? 
                        item.GeographicalPoints.Select(s => new GeographicalPointInput { Latitude = s.Position.Latitude, Longitude = s.Position.Longitude }).ToArray() : 
                        new List<GeographicalPointInput>().ToArray();

                    await agroManager.Barrack.Save(new BarrackInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        GeographicalPoints = geoPoints,
                        Hectares = item.Hectares,
                        IdPlotLand = item.IdPlotLand,
                        IdPollinator = item.IdPollinator,
                        IdRootstock = item.IdRootstock,
                        IdVariety = item.IdVariety,
                        NumberOfPlants = item.NumberOfPlants,
                        PlantingYear = item.PlantingYear,
                        SeasonId = item.SeasonId
                    });
                }
            }
        }
    }
}
