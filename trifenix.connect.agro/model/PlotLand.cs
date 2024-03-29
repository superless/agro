﻿using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    //Entidad encargada de generar las parcelas
    
    [ReferenceSearchHeader(EntityRelated.PLOTLAND, PathName = "plotlands",Kind = EntityKind.ENTITY)]
    
    public class PlotLand : DocumentDb
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Nombre de la parcela.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        /// <summary>
        /// Sector relacionado.
        /// </summary>
        [ReferenceSearch(EntityRelated.SECTOR)]
        public string IdSector { get; set; }

    }
}
