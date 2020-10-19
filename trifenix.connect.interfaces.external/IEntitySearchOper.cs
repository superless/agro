using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.log;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.interfaces.external
{


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntitySearchOper<T> : ILogSimpleQuery {


        /// <summary>
        /// Obtiene un entitySearch desde un elemento de la base de datos.
        /// </summary>
        /// <typeparam name="T2">elemento de la base de datos</typeparam>
        /// <param name="model"></param>
        /// <returns>una colección de entitySearch que representa un elemento de la base de datos</returns>
        IEntitySearch<T>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentBase;



        /// <summary>
        /// Obtiene una colección de entiySearch desde un input (normalmente un input desde la web, que es utilizado para guardar un elemento en la base de datos).
        /// </summary>
        /// <typeparam name="T2">Tio de dato del Input a convertir</typeparam>
        /// <param name="model">input a convertir</param>
        /// <returns></returns>
        IEntitySearch<T>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase;


        /// <summary>
        /// Añade un elemento de la base de datos 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="document"></param>
        void AddDocument<T2>(T2 document) where T2 : DocumentBase;

        

    }

}