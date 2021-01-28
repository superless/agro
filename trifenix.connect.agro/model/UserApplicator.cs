using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Usuario aplicador, 
    /// a diferencia de un usuario este tiene asignado un tractor y una nebulizadora.
    /// </summary>
    public class UserApplicator : User
    {
        /// <summary>
        /// Si el compilador no es "Connect" utilizara un entityname especifico
        /// </summary>
        public UserApplicator() : base()
        {
            #if !CONNECT
               CosmosEntityName = "User"; 
            #endif
        }

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
