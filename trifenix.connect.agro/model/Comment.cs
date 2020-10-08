using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.agro_model
{

    [SharedCosmosCollection("agro", "Comment")]
    public class Comment : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        public string Commentary { get; set; }

        public DateTime Created { get; set; }

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }


    }
}
