using Cosmonaut;
using Microsoft.Graph;
using Moq;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro_model;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;
using trifenix.connect.util;

namespace trifenix.connect.agro.tests.mock
{

   
    /// <summary>
    /// Mocking de base de datos 
    /// mocking de todas las operaciones de la base de datos,
    /// usa AgroData para mantener el registro de datos en cada operación.
    /// </summary>
    public class MockConnect : IDbAgroConnect
    {
        
        
        

        /// <summary>
        /// Mock de consultas agricolas a la base de datos.
        /// </summary>
        public ICommonAgroQueries CommonQueries
        {
            get
            {
                var mock = new Mock<ICommonAgroQueries>();
                // dosis activas sacadas desde agroData.
                mock.Setup(s => s.GetActiveDosesIdsFromProductId(It.IsAny<string>())).ReturnsAsync((string id) => AgroData.Doses.Where(s => s.IdProduct.Equals(id) && s.Active).Select(s => s.Id).ToList());


                // obtiene un season id desde un barrack
                mock.Setup(s => s.GetSeasonId(It.IsAny<string>())).ReturnsAsync((string id) => AgroData.Barracks.FirstOrDefault(s => s.Id.Equals(id))?.SeasonId);


                //obtiene la dosis por defecto de un producto.
                mock.Setup(s => s.GetDefaultDosesId(It.IsAny<string>())).ReturnsAsync((string id) => {

                    var dosesDefault = AgroData.Doses.FirstOrDefault(s => s.IdProduct.Equals(id) && s.Active)?.Id;
                    return dosesDefault ?? string.Empty;

                });


                return mock.Object;
            }
        }

        /// <summary>
        /// Mock a la api de microsoft para gestión de identidades.
        /// </summary>
        public IGraphApi GraphApi
        {
            get
            {
                var mock = new Mock<IGraphApi>();
                // definición de métodos.



                return mock.Object;
            }
        }

        /// <summary>
        /// Mock de operaciones de base de datos, donde residen consultas de existencia.
        /// </summary>
        public IDbExistsElements GetDbExistsElements => MockHelper.GetExistElement();


        

        /// <summary>
        /// Mock de conversiones de base de datos, cosmos usa un método estático para convertir un IQueriable a lista,
        /// para testear se debe mockear.
        /// </summary>
        /// <typeparam name="T">Tipo de base de datos de persistencia</typeparam>
        /// <returns></returns>
        public ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase
        {
            var mock = new Mock<ICommonDbOperations<T>>();
            // definición de métodos.
           

            return mock.Object;
        }


        /// <summary>
        /// Añade un elemento a una colección de AgroData, de acuerdo al tipo de elemento a insertar.
        /// </summary>
        /// <typeparam name="T">Tipo de elemento a insertar</typeparam>
        /// <param name="element">elemento a insertar en una de las colecciones</param>
        private static void AddElement<T>(T element) where T : DocumentBase {

            // obtiene el índice desde mdm, utiliza el atributo para identificar a que entitySearch pertenece.
            var index = Mdm.GetIndex(typeof(T));

            // si no tiene el atributo, lanzará error.
            if (!index.HasValue) throw new System.Exception("el objeto debe tener el atributo ReferenceHeader");

            // de acuerdo al entityRelated insertaremos en la colección.
            var entityRelated = (EntityRelated)index.Value;

            switch (entityRelated)
            {
                case EntityRelated.WAITINGHARVEST:
                    // no es un elemento de base de datos
                    break;
                case EntityRelated.BARRACK:
                    AgroData.Barracks = Mdm.Reflection.Collections.UpsertToCollection((Barrack)(object)element, AgroData.Barracks);
                    break;
                case EntityRelated.BUSINESSNAME:
                    AgroData.BusinessNames = Mdm.Reflection.Collections.UpsertToCollection((BusinessName)(object)element, AgroData.BusinessNames);
                    break;
                case EntityRelated.CATEGORY_INGREDIENT:
                    AgroData.IngredientCategories = Mdm.Reflection.Collections.UpsertToCollection((IngredientCategory)(object)element, AgroData.IngredientCategories);
                    break;
                case EntityRelated.CERTIFIED_ENTITY:
                    AgroData.CertifiedEntities = Mdm.Reflection.Collections.UpsertToCollection((CertifiedEntity)(object)element, AgroData.CertifiedEntities);
                    break;
                case EntityRelated.COSTCENTER:
                    AgroData.CostCenters = Mdm.Reflection.Collections.UpsertToCollection((CostCenter)(object)element, AgroData.CostCenters);
                    break;
                case EntityRelated.DOSES:
                    AgroData.Doses = Mdm.Reflection.Collections.UpsertToCollection((Dose)(object)element, AgroData.Doses);
                    break;
                case EntityRelated.INGREDIENT:
                    AgroData.Ingredients = Mdm.Reflection.Collections.UpsertToCollection((Ingredient)(object)element, AgroData.Ingredients);
                    break;
                case EntityRelated.JOB:
                    break;
                case EntityRelated.NEBULIZER:
                    break;
                case EntityRelated.PHENOLOGICAL_EVENT:
                    break;
                case EntityRelated.PLOTLAND:
                    AgroData.PlotLands = Mdm.Reflection.Collections.UpsertToCollection((PlotLand)(object)element, AgroData.PlotLands);
                    break;
                case EntityRelated.PRODUCT:
                    AgroData.Products = Mdm.Reflection.Collections.UpsertToCollection((Product)(object)element, AgroData.Products);
                    break;
                case EntityRelated.ROLE:
                    break;
                case EntityRelated.ROOTSTOCK:
                    AgroData.Rootstocks = Mdm.Reflection.Collections.UpsertToCollection((Rootstock)(object)element, AgroData.Rootstocks);
                    break;
                case EntityRelated.SEASON:
                    AgroData.Seasons = Mdm.Reflection.Collections.UpsertToCollection((Season)(object)element, AgroData.Seasons);
                    break;
                case EntityRelated.SECTOR:
                    AgroData.Sectors = Mdm.Reflection.Collections.UpsertToCollection((Sector)(object)element, AgroData.Sectors);
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
                    AgroData.Varieties = Mdm.Reflection.Collections.UpsertToCollection((Variety)(object)element, AgroData.Varieties);
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
                    AgroData.Species = Mdm.Reflection.Collections.UpsertToCollection((Specie)(object)element, AgroData.Species);
                    break;
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    AgroData.Brands = Mdm.Reflection.Collections.UpsertToCollection((Brand)(object)element, AgroData.Brands);
                    break;
                default:
                    throw new System.Exception("not good");
                    
            }
            
        }


        /// <summary>
        /// Obtiene un elemento de la colección AgroData, de acuerdo al tipo, seleccionará la colección correspondente para retornar el objeto.
        /// </summary>
        /// <typeparam name="T">Tipo de elemento a retornar</typeparam>
        /// <param name="id">identificador del elemento</param>
        /// <returns></returns>
        private static T GetElement<T>(string id)
        {

            // índice de la clase, de acuerdo al modelo mdm.
            var index = Mdm.GetIndex(typeof(T));

            // excepción si no tiene el atributo.
            if (!index.HasValue) throw new System.Exception("el objeto debe tener el atributo ReferenceHeader");

            // de acuerdo al entityRelated entregará el elemento.
            var entityRelated = (EntityRelated)index.Value;

            switch (entityRelated)
            {
                case EntityRelated.WAITINGHARVEST:
                    throw new Exception("waitin harvest no es un elemento de base de datos, este elemento no hereda de DocumentBase");
                case EntityRelated.BARRACK:
                    return (T)(object)AgroData.Barracks.First(s => s.Id.Equals(id));
                case EntityRelated.BUSINESSNAME:
                    return (T)(object)AgroData.BusinessNames.First(s => s.Id.Equals(id));
                case EntityRelated.CATEGORY_INGREDIENT:
                    return (T)(object)AgroData.IngredientCategories.First(s => s.Id.Equals(id));
                case EntityRelated.CERTIFIED_ENTITY:
                    return (T)(object)AgroData.CertifiedEntities.First(s => s.Id.Equals(id));
                case EntityRelated.COSTCENTER:
                    return (T)(object)AgroData.CostCenters.First(s => s.Id.Equals(id));
                case EntityRelated.DOSES:
                    return (T)(object)AgroData.Doses.First(s => s.Id.Equals(id));
                case EntityRelated.INGREDIENT:
                    return (T)(object)AgroData.Ingredients.First(s => s.Id.Equals(id));
                case EntityRelated.JOB:
                    break;
                case EntityRelated.NEBULIZER:
                    break;
                case EntityRelated.PHENOLOGICAL_EVENT:
                    break;
                case EntityRelated.PLOTLAND:
                    break;
                case EntityRelated.PRODUCT:
                    return (T)(object)AgroData.Products.First(s => s.Id.Equals(id));
                case EntityRelated.ROLE:
                    break;
                case EntityRelated.ROOTSTOCK:
                    return (T)(object)AgroData.Rootstocks.First(s => s.Id.Equals(id));
                case EntityRelated.SEASON:
                    return (T)(object)AgroData.Seasons.First(s => s.Id.Equals(id));
                case EntityRelated.SECTOR:
                    return (T)(object)AgroData.Sectors.First(s => s.Id.Equals(id));
                case EntityRelated.PREORDER:
                    break;
                case EntityRelated.TARGET:
                    break;
                case EntityRelated.TRACTOR:
                    break;
                case EntityRelated.USER:
                    break;
                case EntityRelated.VARIETY:
                    return (T)(object)AgroData.Varieties.First(s => s.Id.Equals(id));
                    
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
                    return (T)(object)AgroData.Species.First(s => s.Id.Equals(id));
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    return (T)(object)AgroData.Brands.First(s => s.Id.Equals(id));
                default:
                    throw new Exception("not good");
            }
            throw new Exception("not good");
        }


        /// <summary>
        /// Borra un elemento de una colección de AgroData, 
        /// la colección esta determinada por el tipo de dato.
        /// </summary>
        /// <typeparam name="T">Tipo de dato a eliminar</typeparam>
        /// <param name="id">identificador del elemento a eliminar</param>
        private static void RemoveElementFromDb<T>(string id) {

            // índice del tipo en el modelo mdm.
            var index = Mdm.GetIndex(typeof(T));

            // si no tiene atributo
            if (!index.HasValue) throw new System.Exception("el objeto debe tener el atributo ReferenceHeader");


            // de acuerdo al tipo de entityRelated eliminará el elemento.
            var entityRelated = (EntityRelated)index.Value;

            switch (entityRelated)
            {
                case EntityRelated.WAITINGHARVEST:
                    AgroData.WaitingHarvest = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.WaitingHarvest);
                    break;
                case EntityRelated.BARRACK:
                    AgroData.Barracks = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Barracks);
                    break;
                case EntityRelated.BUSINESSNAME:
                    AgroData.BusinessNames = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.BusinessNames);
                    break;
                case EntityRelated.CATEGORY_INGREDIENT:
                    AgroData.IngredientCategories = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.IngredientCategories);
                    break;
                case EntityRelated.CERTIFIED_ENTITY:
                    AgroData.CertifiedEntities = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.CertifiedEntities);
                    break;
                case EntityRelated.COSTCENTER:
                    AgroData.CostCenters = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.CostCenters);
                    break;
                case EntityRelated.DOSES:
                    AgroData.Doses = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Doses);
                    break;
                case EntityRelated.INGREDIENT:
                    AgroData.Ingredients = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Ingredients);
                    break;
                case EntityRelated.JOB:
                    break;
                case EntityRelated.NEBULIZER:
                    break;
                case EntityRelated.PHENOLOGICAL_EVENT:
                    break;
                case EntityRelated.PLOTLAND:
                    break;
                case EntityRelated.PRODUCT:
                    AgroData.Products = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Products);
                    break;
                case EntityRelated.ROLE:
                    break;
                case EntityRelated.ROOTSTOCK:
                    AgroData.Rootstocks = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Rootstocks);
                    break;
                case EntityRelated.SEASON:
                    AgroData.Seasons = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Seasons);
                    break;
                case EntityRelated.SECTOR:
                    AgroData.Sectors = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Sectors);
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
                    AgroData.Varieties = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Varieties);
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
                    AgroData.Species = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Species);
                    break;
                case EntityRelated.GEOPOINT:
                    break;
                case EntityRelated.BRAND:
                    AgroData.Brands = Mdm.Reflection.Collections.DeleteElementInCollection(id, AgroData.Brands);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Mock del objeto principal de base de datos
        /// </summary>
        /// <typeparam name="T">Tipo de base de datos de persistencia</typeparam>
        /// <returns>MainGenericDb</returns>
        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase
        {
            var mock = new Mock<IMainGenericDb<T>>();

           
            // definición de métodos.
            // borra una entidad de una base de datos.
            mock.Setup(s => s.DeleteEntity(It.IsAny<string>())).Callback((string id) =>
            {
                RemoveElementFromDb<T>(id);

            });


            // añade elementos a una base de datos.
            mock.Setup(s => s.CreateUpdate(It.IsAny<T>())).Callback((T s) => {
                AddElement(s);
            });


            // obtiene un elemento de una base de datos.
            mock.Setup(s => s.GetEntity(It.IsAny<string>())).ReturnsAsync((string id) => GetElement<T>(id));

            return mock.Object;
        }


        /// <summary>
        /// Validaciones de elementos input
        /// </summary>
        /// <typeparam name="T_INPUT"></typeparam>
        /// <typeparam name="T_DB"></typeparam>
        /// <param name="isBatch"></param>
        /// <returns></returns>
        public IValidatorAttributes<T_INPUT> GetValidator<T_INPUT, T_DB>()
            where T_INPUT : InputBase
            where T_DB : DocumentBase
        {
            return new MainValidator<T_DB, T_INPUT>(GetDbExistsElements);
        }
    }
}
