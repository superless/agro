using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;

using trifenix.agro.external.operations;
using trifenix.agro.search.operations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.az_search;
using trifenix.connect.mdm.enums;

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
                        var dosesEntities = dosesProductId.Select(searchServiceInstance.GetEntitySearch).ToList();
                        listEntitySearch.AddRange(dosesEntities);
                    }


                    var relatedIdsDoseByProduct = listEntitySearch.Where(s => s.index == (int)EntityRelated.DOSES).Select(s =>
                        new RelatedId
                        {
                            id = s.id,
                            index = (int)EntityRelated.DOSES
                        }
                    );

                    var product = searchServiceInstance.GetEntitySearch(item);
                    var related = product.rel.ToList();

                    related.AddRange(relatedIdsDoseByProduct);
                    product.rel = related.ToArray();

                    listEntitySearch.Add(product);

                    var searchs = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElements(listEntitySearch.ToList());

                }
            }


            if (targets.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in targets.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }



            if (ingredients.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in ingredients.Result)
                {
                   
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }


            if (jobs.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in jobs.Result)
                {
                   
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }

            if (costCenter.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in costCenter.Result)
                {
                   
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }

      

            if (roles.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in roles.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
                
            }


            if (plotlands.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in plotlands.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
                
            }



            if (certifiedEntity.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in certifiedEntity.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }

                
            }

            if (rootStocks.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in rootStocks.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }

                
            }

            if (sectors.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in sectors.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
                
            }


            if (seasons.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in seasons.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }


            if (businessNames.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in businessNames.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }

            if (species.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in species.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }

            if (varieties.StatusResult == ExtGetDataResult.Success)
            {
                foreach (var item in varieties.Result)
                {
                    
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }

            if (barracks.StatusResult == ExtGetDataResult.Success)
            {
                
                foreach (var item in barracks.Result)
                {  
                    var search = searchServiceInstance.GetEntitySearch(item);
                    searchServiceInstance.AddElement(search);
                }
            }
        }
    }
}
