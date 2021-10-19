using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de marcas
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.BRAND)]
    public class BrandInput : LocalBase
    {
        /// <summary>
        /// Nombre de la marca
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }



}