using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Comment")]
    public class Comment : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Commentary { get; set; }

        public DateTime Created { get; set; }

        public string IdUser { get; set; }

        public string EntityName { get; set; }

        public string EntityId { get; set; }


    }
}
