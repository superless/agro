using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Usuario aplicador, 
    /// a diferencia de un usuario este tiene asignado un tractor y una nebulizadora.
    /// </summary>    
    public class UserApplicator : User
    {
        

        /// <summary>
        /// Nebulizadora asignada
        /// </summary>
        [ReferenceSearch(EntityRelated.NEBULIZER)]
        public string IdNebulizer { get; set; }

        /// <summary>
        /// Identificador de un tractor.
        /// </summary>
        [ReferenceSearch(EntityRelated.TRACTOR)]
        public string IdTractor { get; set; }

    }
}
