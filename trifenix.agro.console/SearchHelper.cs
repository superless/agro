using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.model;
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



            

            var agroManager = new AgroManager(dbArguments, null, null, null, searchServiceInstance, "ba7e86c8-6c2d-491d-bb2e-0dd39fdf5dc1", false);

           
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

            var doses = await agroManager.Dose.GetElements();






            if (products.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in products.Result)
                {
                    
                    var dosesProductId = doses.Result.Where(s => s.IdProduct.Equals(item.Id) && s.Active).ToList();
                    var listEntitySearch = new List<EntitySearch>();

                    if (dosesProductId.Any())
                    {
                        var dosesEntities = dosesProductId.SelectMany(searchServiceInstance.GetEntitySearch).ToList();
                        listEntitySearch.AddRange(dosesEntities);
                    }


                    var relatedIdsDoseByProduct = listEntitySearch.Where(s => s.EntityIndex.Contains((int)EntityRelated.DOSES)).Select(s =>
                        new RelatedId
                        {
                            EntityId = s.Id,
                            EntityIndex = s.EntityIndex.FirstOrDefault(a=>a == (int)EntityRelated.DOSES)
                        }
                    );

                    var entityProducts = searchServiceInstance.GetEntitySearch(item);

                    var product = entityProducts.FirstOrDefault(s => s.EntityIndex.Contains((int)EntityRelated.PRODUCT));
                    var related = product.RelatedIds.ToList();
                    related.AddRange(relatedIdsDoseByProduct);
                    product.RelatedIds = related.ToArray();
                    listEntitySearch.AddRange(entityProducts);

                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(listEntitySearch.ToList());

                }
            }


            if (targets.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in targets.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }



            if (ingredients.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in ingredients.Result)
                {
                   
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }


            if (jobs.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in jobs.Result)
                {
                   
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }

            if (costCenter.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in costCenter.Result)
                {
                   
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
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
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
                
            }


            if (plotlands.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in plotlands.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
                
            }



            if (certifiedEntity.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in certifiedEntity.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }

                
            }

            if (rootStocks.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in rootStocks.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }

                
            }

            if (sectors.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in sectors.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
                
            }


            if (seasons.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in seasons.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }


            if (businessNames.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in businessNames.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }

            if (species.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in species.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }

            if (varieties.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in varieties.Result)
                {
                    
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }

            if (barracks.StatusResult == ExtGetDataResult.Success)
            {
                
                foreach (var item in barracks.Result)
                {  
                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(searchs.ToList());
                }
            }
        }
    }
}
