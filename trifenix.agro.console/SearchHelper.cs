using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.enums;
using trifenix.agro.external.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.console
{
    public static class SearchHelper
    {
        public static async Task UpdateSearch(AgroDbArguments dbArguments, IAgroSearch searchServiceInstance)
        {


            var agroManager = new AgroManager(dbArguments, null, null, null, null, searchServiceInstance);

            var species = await agroManager.Species.GetElements();

            var varieties = await agroManager.Varieties.GetElements();

            var barracks = await agroManager.Barracks.GetElements();

            var businessNames = await agroManager.BusinessNames.GetElements();

            var seasons = await agroManager.Seasons.GetElements();

            var sectors = await agroManager.Sectors.GetElements();

            var rootStocks = await agroManager.Rootstock.GetElements();

            var certifiedEntity = await agroManager.CertifiedEntities.GetElements();

            var plotlands = await agroManager.PlotLands.GetElements();

            var roles = await agroManager.Roles.GetElements();

            var users = await agroManager.Users.GetElements();

            var costCenter = await agroManager.CostCenters.GetElements();

            var jobs = await agroManager.Jobs.GetElements();

            if (jobs.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in jobs.Result)
                {
                    await agroManager.Jobs.Save(new JobInput
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
                    await agroManager.CostCenters.Save(new CostCenterInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IdBusinessName = item.IdBusinessName
                        
                    });
                }
            }

            if (users.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in users.Result)
                {
                    await agroManager.Users.Save(new UserApplicatorInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Email = item.Email,
                        IdJob = item.IdJob,
                        IdNebulizer = item.IdNebulizer,
                        IdsRoles = item.IdsRoles,
                        IdTractor = item.IdTractor,
                        Rut = item.Rut
                    });
                }
            }


            if (roles.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in roles.Result)
                {
                    await agroManager.Roles.Save(new RoleInput
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
                    await agroManager.PlotLands.Save(new PlotLandInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IdSector = item.IdSector


                    });
                }
            }


            if (plotlands.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in plotlands.Result)
                {
                    await agroManager.PlotLands.Save(new PlotLandInput
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
                    await agroManager.CertifiedEntities.Save(new CertifiedEntityInput
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
                    await agroManager.Rootstock.Save(new RootStockInput
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
                    await agroManager.Sectors.Save(new SectorInput
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
                    await agroManager.Seasons.Save(new SeasonInput
                    {
                        Id = item.Id,
                        Current = item.Current,
                        EndDate = item.End,
                        StartDate = item.Start,
                        IdCostCenter = item.IdCostCenter

                    });
                }
            }


            if (businessNames.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in businessNames.Result)
                {
                    await agroManager.BusinessNames.Save(new BusinessNameInput
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
                    await agroManager.Species.Save(new SpecieInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Name
                    });
                }
            }

            if (species.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in varieties.Result)
                {
                    await agroManager.Varieties.Save(new VarietyInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Abbreviation = item.Name,
                        IdSpecie = item.IdSpecie
                    });
                }
            }

            if (species.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in barracks.Result)
                {
                    await agroManager.Barracks.Save(new BarrackInput
                    {
                        Id = item.Id,
                        Name = item.Name,
                        GeographicalPoints = item.GeographicalPoints,
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
