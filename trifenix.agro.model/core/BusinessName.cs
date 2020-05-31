using Cosmonaut;
using Cosmonaut.Attributes;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.core {

    [SharedCosmosCollection("agro", "BusinessName")]
    [ReferenceSearchHeader(EntityRelated.BUSINESSNAME, PathName = "business_names", Kind =EntityKind.ENTITY)]
    
    public class BusinessName : DocumentBaseName<long>, ISharedCosmosEntity {
        
        public override string Id { get; set; }


        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        [Group(0,Device.WEB, 6)]
        public override string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_EMAIL)]
        [Group(0, Device.WEB, 6)]
        public string Email { get; set; }

        [StringSearch(StringRelated.GENERIC_RUT)]
        [Group(1, Device.WEB, 3)]
        public string Rut { get; set; }

        [Group(1, Device.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_WEBPAGE)]
        public string WebPage { get; set; }

        [Group(1, Device.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_GIRO)]
        public string Giro { get; set; }

        [Group(1, Device.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_PHONE)]
        public string Phone { get; set; }

    }

}