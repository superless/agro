using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de parcelas
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.PLOTLAND)]
    public class PlotLandInput : LocalBase
    {

        /// <summary>
        /// Búsqueda por referencia del sector asociado
        /// </summary>
        [Required, Reference(typeof(Sector))]
        [ReferenceSearch(EntityRelated.SECTOR)]
        public string IdSector { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }

  

}