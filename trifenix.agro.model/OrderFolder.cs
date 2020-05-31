using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    [ReferenceSearchHeader(EntityRelated.ORDER_FOLDER, PathName = "order_folders", Kind = EntityKind.ENTITY)]
    public class OrderFolder : DocumentBase<long>, ISharedCosmosEntity
    {


        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }


        [ReferenceSearch(EntityRelated.TARGET)]
        public string IdApplicationTarget { get; set; }


        [ReferenceSearch(EntityRelated.SPECIE)]
        public string IdSpecie { get; set; }


        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string  IdIngredient { get; set; }


        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string IdIngredientCategory { get; set; }
    }
}