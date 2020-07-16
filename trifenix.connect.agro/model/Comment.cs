using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.db;

namespace trifenix.connect.agro.model
{

    [SharedCosmosCollection("agro", "Comment")]
    public class Comment : DocumentBase<long>, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public override long ClientId { get; set; }


        public string Commentary { get; set; }

        public DateTime Created { get; set; }

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }


    }
}
