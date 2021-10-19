using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de actividad de usuario
    /// </summary>
    public class UserActivityInput : LocalBase
    {

        /// <summary>
        /// Acción a realizar
        /// </summary>
        [Required]
        public UserActivityAction Action { get; set; }

        /// <summary>
        /// Fecha donde fue realizada
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Nombre de entidad
        /// </summary>
        [Required]
        public string EntityName { get; set; }
        
        /// <summary>
        /// Id de identidad
        /// </summary>
        [Required]
        public string EntityId { get; set; }

    }

}