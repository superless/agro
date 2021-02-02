using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using trifenix.agro.external;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.queries;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro_model;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.search;
using trifenix.connect.interfaces.upload;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.tests.mock;
using trifenix.connect.util;

namespace trifenix.connect.agro.tests.mock
{

    /// <summary>
    /// Mocks de las llamadas a base de datos u otras conexiones.
    /// </summary>
    public static class MockHelper
    {


        /// <summary>
        /// AgroSearch usando mock de base de datos de busqueda
        /// </summary>
        /// <returns>AgroSearch</returns>
        public static IAgroSearch<GeoPointTs> AgroSearch() => new AgroSearch<GeoPointTs>(BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());

        /// <summary>
        /// AgroSearch usando mock de base de datos de busqueda
        /// </summary>
        /// <returns>AgroSearch</returns>
        public static IAgroSearch<GeoPointTs> AgroSearch(IBaseEntitySearch<GeoPointTs> baseSearch) => new AgroSearch<GeoPointTs>(baseSearch, new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());



        /// <summary>
        /// Retorna el agromanager por defecto para los tests.
        /// </summary>
        public static AgroManager<GeoPointTs> AgroManager =>  new AgroManager<GeoPointTs>(Connect(), Email(), UploadImage(), WeatherApi(), AgroSearch(BaseSearch()), string.Empty);



        private static IEntitySearch<GeoPointTs>[] ReturnSearchElement(string id, EntityRelated index)
        {

            switch (index)
            {
                case EntityRelated.WAITINGHARVEST:
                    return AgroData.WaitingHarvestSearch.Where(s=>Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                    
                case EntityRelated.BARRACK:
                    return AgroData.BarracksSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.BUSINESSNAME:
                    return AgroData.BusinessNamesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                    
                case EntityRelated.CATEGORY_INGREDIENT:
                    return AgroData.IngredientCategoriesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.CERTIFIED_ENTITY:
                    return AgroData.CertifiedEntitiesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();                    
                case EntityRelated.COSTCENTER:
                    return AgroData.CostCentersSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.DOSES:
                    return AgroData.DosesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.INGREDIENT:
                    return AgroData.IngredientsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();                    
                case EntityRelated.JOB:
                    return AgroData.JobsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.NEBULIZER:
                    return AgroData.NebulizersSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.PHENOLOGICAL_EVENT:
                    return AgroData.PhenologicalEventsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.PLOTLAND:
                    return AgroData.PlotLandsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();                    
                case EntityRelated.PRODUCT:
                    return AgroData.ProductSearchs.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();                    
                case EntityRelated.ROLE:
                    return AgroData.RolesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.ROOTSTOCK:
                    return AgroData.RootstocksSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.SEASON:
                    return AgroData.SeasonsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.SECTOR:
                    return AgroData.SectorsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();                    
                case EntityRelated.PREORDER:
                    return AgroData.PreOrdersSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.TARGET:
                    return AgroData.ApplicationTargetsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.TRACTOR:
                    return AgroData.TractorsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.USER:
                    break;
                case EntityRelated.VARIETY:
                    return AgroData.VarietiesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.NOTIFICATION_EVENT:
                    break;
                case EntityRelated.POLLINATOR:
                    break;
                case EntityRelated.ORDER_FOLDER:
                    break;
                case EntityRelated.EXECUTION_ORDER:
                    break;
                case EntityRelated.ORDER:
                    break;
                case EntityRelated.BARRACK_EVENT:
                    break;
                case EntityRelated.DOSES_ORDER:
                    break;
                case EntityRelated.EXECUTION_ORDER_STATUS:
                    break;
                case EntityRelated.SPECIE:
                    return AgroData.SpeciesSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    return AgroData.BrandsSearch.Where(s => Mdm.Reflection.Collections.GetId(s).Equals(id)).ToArray();
                default:
                    break;
            }

            throw new Exception("not good");
        }

        private static void RemoveSearchElement(string id, EntityRelated index) {
            
            switch (index)
            {
                case EntityRelated.WAITINGHARVEST:
                    AgroData.WaitingHarvestSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.WaitingHarvestSearch);
                    break;
                case EntityRelated.BARRACK:
                    AgroData.BarracksSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.BarracksSearch);
                    break;
                case EntityRelated.BUSINESSNAME:
                    AgroData.BusinessNamesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.BusinessNamesSearch);
                    break;
                case EntityRelated.CATEGORY_INGREDIENT:
                    AgroData.IngredientCategoriesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.IngredientCategoriesSearch);
                    break;
                case EntityRelated.CERTIFIED_ENTITY:
                    AgroData.CertifiedEntitiesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.CertifiedEntitiesSearch);
                    break;
                case EntityRelated.COSTCENTER:
                    AgroData.CostCentersSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.CostCentersSearch);
                    break;
                case EntityRelated.DOSES:
                    AgroData.DosesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.DosesSearch);
                    break;
                case EntityRelated.INGREDIENT:
                    AgroData.IngredientsSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.IngredientsSearch);
                    break;
                case EntityRelated.JOB:
                    break;
                case EntityRelated.NEBULIZER:
                    break;
                case EntityRelated.PHENOLOGICAL_EVENT:
                    break;
                case EntityRelated.PLOTLAND:
                    AgroData.PlotLandsSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.PlotLandsSearch);
                    break;
                case EntityRelated.PRODUCT:
                    AgroData.ProductSearchs = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.ProductSearchs);
                    break;
                case EntityRelated.ROLE:
                    break;
                case EntityRelated.ROOTSTOCK:
                    AgroData.RootstocksSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.RootstocksSearch);
                    break;
                case EntityRelated.SEASON:
                    AgroData.SeasonsSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.SeasonsSearch);
                    break;
                case EntityRelated.SECTOR:
                    AgroData.SectorsSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.SectorsSearch);
                    break;
                case EntityRelated.PREORDER:
                    break;
                case EntityRelated.TARGET:
                    break;
                case EntityRelated.TRACTOR:
                    break;
                case EntityRelated.USER:
                    break;
                case EntityRelated.VARIETY:
                    AgroData.VarietiesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.VarietiesSearch);
                    break;
                case EntityRelated.NOTIFICATION_EVENT:
                    break;
                case EntityRelated.POLLINATOR:
                    break;
                case EntityRelated.ORDER_FOLDER:
                    break;
                case EntityRelated.EXECUTION_ORDER:
                    break;
                case EntityRelated.ORDER:
                    break;
                case EntityRelated.BARRACK_EVENT:
                    break;
                case EntityRelated.DOSES_ORDER:
                    break;
                case EntityRelated.EXECUTION_ORDER_STATUS:
                    break;
                case EntityRelated.SPECIE:
                    AgroData.SpeciesSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.SpeciesSearch);
                    break;
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    AgroData.BrandsSearch = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.BrandsSearch);
                    break;
                default:
                    break;
            }
        }

        private static void AddSearchElement(IEntitySearch<GeoPointTs> input) {
            var index = (EntityRelated)input.index;
            switch (index)
            {
                case EntityRelated.WAITINGHARVEST:
                    AgroData.WaitingHarvestSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.WaitingHarvestSearch);
                    break;
                case EntityRelated.BARRACK:
                    AgroData.BarracksSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.BarracksSearch);
                    break;
                case EntityRelated.BUSINESSNAME:
                    AgroData.BusinessNamesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.BusinessNamesSearch);
                    break;
                case EntityRelated.CATEGORY_INGREDIENT:
                    AgroData.IngredientCategoriesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.IngredientCategoriesSearch);
                    break;
                case EntityRelated.CERTIFIED_ENTITY:
                    AgroData.CertifiedEntitiesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.CertifiedEntitiesSearch);
                    break;
                case EntityRelated.COSTCENTER:
                    AgroData.CostCentersSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.CostCentersSearch);
                    break;
                case EntityRelated.DOSES:
                    AgroData.DosesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.DosesSearch);
                    break;
                case EntityRelated.INGREDIENT:
                    AgroData.IngredientsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.IngredientsSearch);
                    break;
                case EntityRelated.JOB:
                    AgroData.JobsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.JobsSearch);
                    break;
                case EntityRelated.NEBULIZER:
                    AgroData.NebulizersSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.NebulizersSearch);
                    break;
                case EntityRelated.PHENOLOGICAL_EVENT:
                    AgroData.PhenologicalEventsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.PhenologicalEventsSearch);
                    break;
                case EntityRelated.PLOTLAND:
                    AgroData.PlotLandsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.PlotLandsSearch);
                    break;
                case EntityRelated.PRODUCT:
                    AgroData.ProductSearchs = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.ProductSearchs);
                    break;
                case EntityRelated.ROLE:
                    AgroData.RolesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.RolesSearch);
                    break;
                case EntityRelated.ROOTSTOCK:
                    AgroData.RootstocksSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.RootstocksSearch);
                    break;
                case EntityRelated.SEASON:
                    AgroData.SeasonsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.SeasonsSearch);
                    break;
                case EntityRelated.SECTOR:
                    AgroData.SectorsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.SectorsSearch);
                    break;
                case EntityRelated.PREORDER:
                    AgroData.PreOrdersSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.PreOrdersSearch);
                    break;
                case EntityRelated.TARGET:
                    AgroData.ApplicationTargetsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.ApplicationTargetsSearch);
                    break;
                case EntityRelated.TRACTOR:
                    AgroData.TractorsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.TractorsSearch);
                    break;
                case EntityRelated.USER:
                    break;
                case EntityRelated.VARIETY:
                    AgroData.VarietiesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.VarietiesSearch);
                    break;
                case EntityRelated.NOTIFICATION_EVENT:
                    break;
                case EntityRelated.POLLINATOR:
                    break;
                case EntityRelated.ORDER_FOLDER:
                    AgroData.OrderFoldersSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.OrderFoldersSearch);
                    break;
                case EntityRelated.EXECUTION_ORDER:
                    break;
                case EntityRelated.ORDER:
                    break;
                case EntityRelated.BARRACK_EVENT:
                    break;
                case EntityRelated.DOSES_ORDER:
                    break;
                case EntityRelated.EXECUTION_ORDER_STATUS:
                    break;
                case EntityRelated.SPECIE:
                    AgroData.SpeciesSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.SpeciesSearch);
                    break;
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    AgroData.BrandsSearch = Mdm.Reflection.Collections.UpsertToCollection(input, AgroData.BrandsSearch);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Retorna el mock del repositorio de busquedas, donde se ingresan los IEntitySeact<T>
        /// </summary>
        /// <returns></returns>
        public static IBaseEntitySearch<GeoPointTs> BaseSearch() {

            var mock = new Mock<IBaseEntitySearch<GeoPointTs>>();

            mock.Setup(s => s.AddElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>())).Callback((List<IEntitySearch<GeoPointTs>> inputs) =>
            {
                foreach (var input in inputs)
                {
                    AddSearchElement(input);
                }
            }
            );
            mock.Setup(s => s.FilterElements(It.IsAny<string>())).Returns((string query) => {

                var regex = new Regex("index eq (.*?) and id eq '(.*?)'");

                if (!regex.IsMatch(query))
                {
                    return new List<IEntitySearch<GeoPointTs>>();
                }

                var matches = regex.Matches(query);

                

                var index = (EntityRelated)int.Parse(matches[0].Groups[1].Value);

                var id = matches[0].Groups[2].Value;

                return ReturnSearchElement(id, index).ToList();
            });


            mock.Setup(s => s.AddElement(It.IsAny<IEntitySearch<GeoPointTs>>())).Callback((IEntitySearch<GeoPointTs> entity)=> {

                AddSearchElement(entity);
            });

            mock.Setup(s => s.DeleteElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>())).Callback((List<IEntitySearch<GeoPointTs>> entitites) =>
            {
                foreach (var entity in entitites)
                {   
                    RemoveSearchElement(entity.id, (EntityRelated)entity.index);
                }
            });

            mock.Setup(s => s.DeleteElements(It.IsAny<string>())).Callback((string query)=> {
                var regex = new Regex("index eq (.*?) and id eq '(.*?)'");

                if (!regex.IsMatch(query))
                {
                    return;
                }

                var matches = regex.Matches(query);



                var index = (EntityRelated)int.Parse(matches[0].Groups[1].Value);

                var id = matches[0].Groups[2].Value;

                RemoveSearchElement(id, index);

            });
            mock.Setup(s => s.EmptyIndex());

            mock.Setup(s => s.CreateOrUpdateIndex());

            return mock.Object;


        }





        /// <summary>
        /// Retorna mock de correo
        /// </summary>
        /// <returns></returns>
        public static IEmail Email()
        {
            var mail = new Mock<IEmail>();

            mail.Setup(s => s.SendEmail(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()));

            return mail.Object;
        }

        /// <summary>
        /// Mock de las consultas de existencia de elementos.
        /// </summary>
        /// <returns></returns>
        public static IDbExistsElements GetExistElement()
        {
            var mockExistElement = new Mock<IDbExistsElements>();
            
            mockExistElement
                .Setup(s => s.ExistsDosesExecutionOrder(It.IsAny<string>())).ReturnsAsync(false);

            mockExistElement
            .Setup(s => s.ExistsDosesFromOrder(It.IsAny<string>())).ReturnsAsync(false);

            mockExistElement
              .Setup(s => s.ExistsById<Dose>(It.IsAny<string>())).ReturnsAsync((string id) => AgroData.Doses.Any(s => s.Id.Equals(id)));

            // Busqueda en colecciones de AgroInputData
            mockExistElement
                .Setup(s => s.ExistsById<Product>(It.IsAny<string>()))

                .ReturnsAsync((string id) => AgroData.Products.Any(s => s.Id.Equals(id)));

            mockExistElement
                .Setup(s => s.ExistsById<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {

                    return AgroData.Ingredients.Any(s => s.Id.Equals(id));
                });

            mockExistElement
                .Setup(s => s.ExistsById<IngredientCategory>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {

                    return AgroData.IngredientCategories.Any(s => s.Id.Equals(id));
                });


            mockExistElement.Setup(s => s.ExistsById<Brand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Brands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Variety>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Varieties.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Specie>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Species.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CertifiedEntity>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.CertifiedEntities.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<ApplicationTarget>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.ApplicationTargets.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Season>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Seasons.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Barrack>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Barracks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Sector>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Sectors.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<PlotLand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.PlotLands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Rootstock>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Rootstocks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CostCenter>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroData.CostCenters.Any(s => s.Id.Equals(id));
               });


            mockExistElement.Setup(s => s.ExistsById<BusinessName>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroData.BusinessNames.Any(s => s.Id.Equals(id));
               });



            // busqueda en colecciones usando nombre propiedad y valor, y si existe un id, excluyendolo de la busqueda.
            mockExistElement
                .Setup(s => s.ExistsWithPropertyValue<Product>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroInputData.Products.Any(s => Mdm.Reflection.Collections.GetProp(s, nameProp).Equals(propValue)
                    &&
                    (string.IsNullOrWhiteSpace(id) || !s.Id.Equals(id))));


            mockExistElement
                .Setup(s => s.ExistsWithPropertyValue<Barrack>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroInputData.Barracks.Any(s => Mdm.Reflection.Collections.GetProp(s, nameProp).Equals(propValue)
                    &&
                    (string.IsNullOrWhiteSpace(id) || !s.Id.Equals(id))
                ));

            // los métodos que usan singleQuery o multipleQuery no necesitan ser mockeados, porque todos son usados en IAgroCommonQueries.


            return mockExistElement.Object;

        }

        /// <summary>
        /// Mock de upload Image
        /// </summary>
        /// <returns></returns>
        public static IUploadImage UploadImage()
        {
            var uimage = new Mock<IUploadImage>();
            uimage.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync("image 01");
            return uimage.Object;
        }


        /// <summary>
        /// Mock de clima.
        /// </summary>
        /// <returns></returns>
        public static IWeatherApi WeatherApi()
        {
            var weatherApi = new Mock<IWeatherApi>();
            weatherApi.Setup(s => s.GetWeather(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(new Weather { });
            return weatherApi.Object;
        }


        // Mock de conexiones a base de datos
        public static IDbAgroConnect Connect() => new MockConnect();

    }
}
