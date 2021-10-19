using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de usuario que realiza actividad
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.USER)]
    public class UserApplicatorInput : LocalBase
    {

        /// <summary>
        /// Nombre
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// RUT
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_RUT)]
        public string Rut { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Unique]
        [StringSearch(StringRelated.GENERIC_EMAIL)]
        public string Email { get; set; }

        /// <summary>
        /// Búsqueda por referencia del trabajo del usuario
        /// </summary>
        [Reference(typeof(Job))]
        [ReferenceSearch(EntityRelated.JOB)]
        public string IdJob { get; set; }

        /// <summary>
        /// Búsqueda por referencia del rol o roles del usuario
        /// </summary>
        [Required, Reference(typeof(Role))]
        [ReferenceSearch(EntityRelated.ROLE)]
        public List<string> IdsRoles { get; set; }

        /// <summary>
        /// Búsqueda por referencia del nebulizador a utilizar
        /// </summary>
        [ReferenceSearch(EntityRelated.NEBULIZER)]
        [Reference(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        /// <summary>
        /// Búsqueda por referencia del tractor a utilizar
        /// </summary>
        [ReferenceSearch(EntityRelated.TRACTOR)]
        [Reference(typeof(Tractor))]
        public string IdTractor { get; set; }
    }

   

}