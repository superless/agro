using System.Collections.Generic;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Resultado de una consulta de entidades.
    /// Los valores incluidos + la consulta, permiten refrescar.
    /// </summary>
    public class CollectionResult
    {
        /// <summary>
        /// Total de registros
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Entidades que retorna
        /// </summary>
        public IEntitySearch<GeoPointTs>[] Entities { get; set; }


        /// <summary>
        /// Generalmente una consulta está relacionado con una página.
        /// </summary>
        public int Current { get; set; }


        /// <summary>
        /// Agrupaciones de busqueda.
        /// </summary>
        public Facet[] Facets { get; set; }

        /// <summary>
        /// Filtros de la consulta
        /// </summary>
        public FilterModel Filter { get; set; }


        /// <summary>
        /// Indice de la propiedad que será usada,
        /// por todas las entidades que estén relacionadas con la entidad principal.
        /// </summary>
        public int IndexPropDefaultName { get; set; }


        /// <summary>
        /// En casos particulares de entidades que no usen la propiedad por defecto,
        /// podrá asignar el índice de la entidad, y el o los índices de propiedad de tipo string
        /// </summary>
        public Dictionary<int, int[]> IndexPropNames { get; set; }



        /// <summary>
        /// Listado de items para el ordenamiento
        /// </summary>
        public List<OrderItem> OrderItems { get; set; }
    }

    

    


}
