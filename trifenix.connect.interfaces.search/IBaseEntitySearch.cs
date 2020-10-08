using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.interfaces.search
{
    public interface IBaseEntitySearch<T>
    {

        /// <summary>
        /// Añade una colección de EntitySearch a azure search
        /// </summary>
        /// <param name="elements">colección de entidades a añadir</param>
        void AddElements(List<IEntitySearch<T>> elements);




        void AddElement(IEntitySearch<T> element);

        void DeleteElements(List<IEntitySearch<T>> elements);

        void DeleteElements(string query);

        void EmptyIndex();

        List<IEntitySearch<T>> FilterElements(string filter);

        void CreateOrUpdateIndex();
    }
}
