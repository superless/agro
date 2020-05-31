using Cosmonaut;
using Cosmonaut.Attributes;
using System.IO;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{
    [SharedCosmosCollection("agro", "PreOrder")]
    [ReferenceSearchHeader(EntityRelated.PREORDER, PathName = "pre-orders", Kind = EntityKind.PROCESS)]
    public class PreOrder : DocumentBaseName<long>, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


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


