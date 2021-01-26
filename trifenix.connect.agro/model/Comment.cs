using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de los comentarios
    /// </summary>
    [SharedCosmosCollection("agro", "Comment")]
    public class Comment : DocumentLocal
    {
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Comentario ingresado
        /// </summary>
        public string Commentary { get; set; }

        /// <summary>
        /// Fecha de creacion del comentario
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }


    }
}
