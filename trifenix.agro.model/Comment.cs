using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "Comment")]
    public class Comment : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Commentary { get; set; }

        public DateTime Created { get; set; }

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }


    }
}
