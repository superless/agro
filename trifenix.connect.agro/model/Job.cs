using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de generar los cargos de los usuarios
    /// </summary>
    [SharedCosmosCollection("agro", "Job")]
    [ReferenceSearchHeader(EntityRelated.JOB, PathName = "jobs", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.USUARIOS)]
    public class Job : DocumentLocal {

        /// <summary>
        /// Identificador visual de cargo
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre del cargo.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }
}
