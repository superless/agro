using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{
    
    public class UserApplicator : User
    {
        public UserApplicator() : base()
        {
            CosmosEntityName = "User";
        }

        [ReferenceSearch(EntityRelated.NEBULIZER)]
        public string IdNebulizer { get; set; }

        [ReferenceSearch(EntityRelated.TRACTOR)]
        public string IdTractor { get; set; }

    }
}
