﻿using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{


    /// <summary>
    /// El centro de costos es la unidad central del sistema,
    /// todos los procesos, la bodega y los cuarteles dependen de un centro de costo.
    /// </summary>    
    [ReferenceSearchHeader(EntityRelated.COSTCENTER, PathName = "cost_centers", Kind = EntityKind.ENTITY)]    
    public class CostCenter : DocumentDb {


        /// <summary>
        /// Identificador del centro de costo
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del centro de costo
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }
        
        /// <summary>
        /// Nombre del centro de costo.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Nombre del negocio.
        /// </summary>
        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }
    }

}