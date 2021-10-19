using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{


    /// <summary>
    /// Determina cual es el objetivo de la aplicación de una órden.
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.TARGET, PathName = "targets", Kind = EntityKind.ENTITY)]    
    public class ApplicationTarget : DocumentDb {

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// El identificador de cliente que será mostrado en el formulario y la vista.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Nombre del objetivo de la aplicación
        /// </summary>
        
        [Required]
        [Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Abreviación del objetivo.
        /// </summary>
        [Unique]        
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}