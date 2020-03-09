using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "PreOrder")]
    public class PreOrder : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public override string Name { get; set; }


        public string IdIngredient { get; set; }    //Este esta en la OrderFolder. Eliminar(?)
        public string OrderFolderId { get; set; }

        public PreOrderType PreOrderType { get; set; }


        public string[] BarracksId { get; set; }

    }
}


