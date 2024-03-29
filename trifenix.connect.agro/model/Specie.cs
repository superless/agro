﻿using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidade encargada de generar las especies,
    /// estas corresponden a los productos plantados/cosechados
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.SPECIE, PathName = "species", Kind = EntityKind.ENTITY )]
    
    public class Specie : DocumentDb {
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Nombre de la especie
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        /// <summary>
        /// Abreviación de la especie.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }
    }

}
