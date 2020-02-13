using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro.core {

    [SharedCosmosCollection("agro", "BusinessName")]
    public class BusinessName : DocumentBase, ISharedCosmosEntity {
        
        public override string Id { get; set; }
        public string Email { get; set; }
        public string Rut { get; set; }
        public string Name { get; set; }
        public string WebPage { get; set; }
        public string Giro { get; set; }
        public string Phone { get; set; }
        public UserActivity Modify { get; set; }
    }

}