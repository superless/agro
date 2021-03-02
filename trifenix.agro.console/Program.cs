using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.model;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model;
using trifenix.connect.arguments;
using trifenix.connect.mdm.search.model;
using trifenix.model;

namespace trifenix.agro.console
{

    class Program {

        /// <summary>
        /// Invoca método genérico dinámicamente(Los tipo de datos se determinan en tiempo de ejecución) y castea el resultado a <typeparamref name="CastType"/>.
        /// </summary>
        /// <typeparam name="CastType">Tipo al que se castea el retorno de InvokeGenericMethodDynamically.</typeparam>
        /// <param name="ClassContainer">Clase que contiene el método genérico</param>
        /// <param name="MethodName">Nombre del método genérico</param>
        /// <param name="GenericType">Tipo de dato usado como genérico</param>
        /// <param name="Obj">Objeto para métodos de instancia</param>
        /// <param name="Parameters">Conjunto de parámetros utilizados por el método genérico</param>
        public static CastType InvokeGenericMethodDynamically<CastType>(Type ClassContainer, string MethodName, Type GenericType, object Obj, object[] Parameters = null) {
            var result = (CastType)InvokeGenericMethodDynamically(ClassContainer, MethodName, GenericType, Obj, Parameters);
            return result;
        }

        /// <summary>
        /// Invoca método genérico dinámicamente(Los tipo de datos se determinan en tiempo de ejecución).
        /// </summary>
        /// <param name="ClassContainer">Clase que contiene el método genérico</param>
        /// <param name="MethodName">Nombre del método genérico</param>
        /// <param name="GenericType">Tipo de dato usado como genérico</param>
        /// <param name="Obj">Objeto para métodos de instancia</param>
        /// <param name="Parameters">Conjunto de parámetros utilizados por el método genérico</param>
        public static object InvokeGenericMethodDynamically(Type ClassContainer, string MethodName, Type GenericType, object Obj, object[] Parameters = null) {
            var method = ClassContainer.GetMethod(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(GenericType);
            var result = genericMethod.Invoke(Obj, Parameters);
            return result;
        }

        static async Task RegenerateIndexFromDB<EntityType>(CommonQueries queriesToDB, AgroSearch<GeographyPoint> searchServiceInstance) where EntityType : DocumentDb =>
            (await queriesToDB.MultipleQuery<EntityType, EntityType>("SELECT * from c")).ToList().ForEach(Entity => searchServiceInstance.AddDocument(Entity));

        static async Task Main(string[] args) {

            var queriesToDB = new CommonQueries(new CosmosDbArguments { EndPointUrl = "https://agro-cosmodb.documents.azure.com:443/" , NameDb = "agro-cosmodb", PrimaryKey = "kaPYpzhFCcG1bk3aC69aX1T2amavVi8TfHmrIMNJuhpYXtIz67PMhwBKctunNzclFBcxypZvcjPUW846YZuvjA==" });
            var searchServiceInstance = new AgroSearch<GeographyPoint>("https://fenix-search.search.windows.net/", "EFF07EE3D5A0C74C2363EC4DDB9710D7", new ImplementsSearch(), new HashEntityAgroSearch());

            var entityTypes = new List<Type>{
                typeof(ApplicationOrder),
                typeof(ApplicationTarget),
                typeof(Barrack),
                typeof(Brand),
                typeof(BusinessName),
                typeof(CertifiedEntity),
                typeof(Comment),
                typeof(CostCenter),
                typeof(Dose),
                typeof(ExecutionOrder),
                typeof(ExecutionOrderStatus),
                typeof(Ingredient),
                typeof(IngredientCategory),
                typeof(Job),
                typeof(Nebulizer),
                typeof(NotificationEvent),
                typeof(PhenologicalEvent),
                typeof(PlotLand),
                typeof(PreOrder),
                typeof(Product),
                typeof(OrderFolder),
                typeof(Role),
                typeof(Season),
                typeof(Sector),
                typeof(Specie),
                typeof(Tractor),
                typeof(Rootstock),
                typeof(UserApplicator),
                typeof(Variety),
                typeof(Warehouse),
                typeof(WarehouseDocument)
            };

            foreach (var entityType in entityTypes)
                await InvokeGenericMethodDynamically<Task>(typeof(Program), "RegenerateIndexFromDB", entityType, null, new object[] { queriesToDB, searchServiceInstance });

        }

    }

}