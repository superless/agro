using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.interfaces.log;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.interfaces.search
{


    /// <summary>
    /// Operaciones básicas de busqueda, para cualquier proyecto.
    /// </summary>
    /// <typeparam name="T">Tipo de dato, que representa un tipo de dato Geo</typeparam>
    public interface IBaseEntitySearch<T> 
    {

        string ServiceName { get;  }

        string ServiceKey { get;  }


        string Index { get;  }
        /// <summary>
        /// Añade una colección de EntitySearch a azure search
        /// </summary>
        /// <param name="elements">colección de entidades a añadir</param>
        void AddElements(List<IEntitySearch<T>> elements);



        /// <summary>
        /// Añade un entitySearch a azure search
        /// </summary>
        /// <param name="element">entitySearch a agregar</param>
        void AddElement(IEntitySearch<T> element);


        /// <summary>
        /// Elimina una lista de entitySearch
        /// </summary>
        /// <param name="elements">Elimina una lista de entitySearch</param>
        void DeleteElements(List<IEntitySearch<T>> elements);


        /// <summary>
        /// Elimina elementos basado en una consulta.
        /// </summary>
        /// <param name="query">consulta que retornará los elementos a eliminar</param>
        void DeleteElements(string query);


        /// <summary>
        /// Vacía el índice asignado en el constructor.
        /// </summary>
        void EmptyIndex();


        /// <summary>
        /// Retorna una colección de entitySearch basado en un filtro.
        /// </summary>
        /// <param name="filter">filtro de la consulta</param>
        /// <returns>colección de entitySearch</returns>
        List<IEntitySearch<T>> FilterElements(string filter);


        /// <summary>
        /// Inicializa o actualiza un indice.
        /// </summary>
        void CreateOrUpdateIndex();
    }

}
