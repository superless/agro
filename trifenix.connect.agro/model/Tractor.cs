using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.entities.cosmos;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de generar los tractores,
    /// utilizados para las tareas del oficio
    /// </summary>
    [SharedCosmosCollection("agro", "Tractor")]
    [ReferenceSearchHeader(EntityRelated.TRACTOR, Kind = EntityKind.ENTITY, PathName = "tractors")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.MAQUINARIA)]
    public class Tractor : DocumentLocal
    {

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        
        /// <summary>
        /// Marca del tractor
        /// </summary>
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        /// <summary>
        /// Código del tractor.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }

}