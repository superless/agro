
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de comentarios
    /// </summary>
    public class CommentInput : LocalBase {

        /// <summary>
        /// Comentario
        /// </summary>
        [Required]
        public string Commentary { get; set; }

        /// <summary>
        /// Id del usuario que realiza el comentario
        /// </summary>
        [Required, Reference(typeof(User))]
        public string IdUser { get; set; }

        /// <summary>
        /// Indice de la entidad
        /// </summary>
        [Required]
        public int EntityIndex { get; set; }

        /// <summary>
        /// Id de la identidad
        /// </summary>
        [Required]
        public string EntityId { get; set; }
    }
    /// <summary>
    /// !!!!
    /// </summary>
    public class CommentSwaggerInput  {

        /// <summary>
        /// Comentario
        /// </summary>
        public string Commentary { get; set; }

        /// <summary>
        /// Id usuario
        /// </summary>
        public string IdUser { get; set; }

        /// <summary>
        /// Indice de la identidad
        /// </summary>
        public int EntityIndex { get; set; }

        /// <summary>
        /// Id de la identidad
        /// </summary>
        public string EntityId { get; set; }
    }

}