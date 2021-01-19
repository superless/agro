using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// usuario aplicador, a diferencia de un usuario este tiene asignado un tractor y una nebulizadora.
    /// </summary>
    public class UserApplicator : User
    {
        public UserApplicator() : base()
        {
            #if !DEBUG
               CosmosEntityName = "User"; 
            #endif
        }

        /// <summary>
        /// nebulizadora asignada
        /// </summary>
        [ReferenceSearch(EntityRelated.NEBULIZER)]
        public string IdNebulizer { get; set; }

        /// <summary>
        /// identificador de un tractor.
        /// </summary>
        [ReferenceSearch(EntityRelated.TRACTOR)]
        public string IdTractor { get; set; }

    }
}
