using System;
using System.Collections.Generic;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Colección de filtros que aplican a una consulta
    /// </summary>
    public class FilterModel {

        /// <summary>
        /// Filtro de tipo string
        /// grupo de filtros agrupados por indice de cada propiedad a filtrar.
        /// </summary>
        public Dictionary<int, FilterBase<string>[]> FilterStr { get; set; }


        /// <summary>
        /// filtro de tipo entidad       
        /// </summary>
        public Dictionary<int, FilterBase<string>[]> FilterEntity { get; set; }


        /// <summary>
        /// fitro de tipo enumeración.
        /// </summary>
        public Dictionary <int, FilterBase<int>[]> EnumFilter { get; set; }

        /// <summary>
        /// filtro de tipo long.
        /// </summary>
        public Dictionary<int, FilterBase<long>[]> LongFilter { get; set; }

        /// <summary>
        /// filtro de tipo número.
        /// </summary>
        public Dictionary <int, FilterBase<int>[]> NumFilter { get; set; }

        /// <summary>
        /// filtro de tipo bool
        /// </summary>
        public Dictionary<int, FilterBase<bool>[]> BoolFilters { get; set; }

        /// <summary>
        /// filtro de tipo date
        /// </summary>
        public Dictionary<int, FilterBase<DateTime>[]> DateFilters { get; set; }

        /// <summary>
        /// filtro de tipo double.
        /// </summary>
        public Dictionary<int, FilterBase<double>[]> DoubleFilters { get; set; }

    }

   

    


}
