using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "PreOrder")]
    [ReferenceSearch(EntityRelated.PREORDER)]
    public class PreOrder : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }


        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdIngredient { get; set; }    //Este esta en la OrderFolder. Eliminar(?), esto puede aplicar cuando no sea una preorden fenologica, en cuyo caso, no existirá una Carpeta.


        [ReferenceSearch(EntityRelated.ORDER_FOLDER)]
        public string OrderFolderId { get; set; }

        [EnumSearch(EnumRelated.PRE_ORDER_TYPE)]
        public PreOrderType PreOrderType { get; set; }

        [ReferenceSearch(EntityRelated.BARRACK)]
        public string[] BarracksId { get; set; }

    }
}


