using Cosmonaut;
using Cosmonaut.Attributes;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.core {

    [SharedCosmosCollection("agro", "BusinessName")]
    [ReferenceSearch(EntityRelated.BUSINESSNAME)]
    
    public class BusinessName : DocumentBaseName, ISharedCosmosEntity {
        
        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_EMAIL)]
        public string Email { get; set; }

        [StringSearch(StringRelated.GENERIC_RUT)]
        public string Rut { get; set; }

        [StringSearch(StringRelated.GENERIC_WEBPAGE)]
        public string WebPage { get; set; }

        [StringSearch(StringRelated.GENERIC_GIRO)]
        public string Giro { get; set; }

        [StringSearch(StringRelated.GENERIC_PHONE)]
        public string Phone { get; set; }

    }

}