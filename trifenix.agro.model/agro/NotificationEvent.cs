using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "NotificationEvent")]
    public class NotificationEvent : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public Barrack Barrack { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }

        public string PicturePath { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }





    }
}
