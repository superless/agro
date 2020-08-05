using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// log de acciones de usuario
    /// esta propiedad solo se guarda en cosmos.
    /// </summary>
    [SharedCosmosCollection("agro", "UserActivity")]
    public class UserActivity : DocumentBase, ISharedCosmosEntity {

        /// <summary>
        /// identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Identificador de usuario
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// acción ejecutada.
        /// </summary>
        public UserActivityAction Action { get; set; }

        
        /// <summary>
        /// fecha de la ejecución.
        /// </summary>
        public DateTime Date { get; set; }



        /// <summary>
        /// nombre de la entidad que se modifico.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// identificador de la entidad.
        /// </summary>
        public string EntityId { get; set; }

    }

}