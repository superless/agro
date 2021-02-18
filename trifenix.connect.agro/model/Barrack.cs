using Cosmonaut.Attributes;
using System.ComponentModel.DataAnnotations;

using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model
{


    /// <summary>
    /// Representa un cuartel.
    /// </summary>
    [SharedCosmosCollection("agro", "Barrack")]
    [ReferenceSearchHeader(EntityRelated.BARRACK, Kind =EntityKind.ENTITY, PathName ="barracks")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.TERRENO)]
    public class Barrack : ItemLocation {

        /// <summary>
        /// Identificador del barrack
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Campo autonumérico que identifica el barrack.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Nombre del cuartel.
        /// </summary>
        [Group(0, PhisicalDevice.WEB, 6)]
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Unique]
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Identificador de la parcela.
        /// </summary>
        [Group(0, PhisicalDevice.WEB, 6)]
        [ReferenceSearch(EntityRelated.PLOTLAND)]        
        public string IdPlotLand { get; set; }


        /// <summary>
        /// Hectareas del cuartel.
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [DoubleSearch(DoubleRelated.HECTARES)]
        public double Hectares { get; set; }


        /// <summary>
        /// Año de plantación.
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [Num32Search(NumRelated.PLANTING_YEAR)]
        public int PlantingYear { get; set; }


        /// <summary>
        /// Número de plantas.
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [Num32Search(NumRelated.NUMBER_OF_PLANTS)]
        public int NumberOfPlants { get; set; }


        /// <summary>
        /// Identificador de variedad
        /// </summary>
        [Group(2, PhisicalDevice.WEB, 3)]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }


        /// <summary> 
        /// Identificador de polinizador.
        /// La variedad y el polinizador son el misma entidad,
        /// para asignar la segunda se usa una referencia local.
        /// importante! 
        /// </summary>
        [Group(2, PhisicalDevice.WEB, 3)]
        [ReferenceSearch(EntityRelated.POLLINATOR, EntityRelated.VARIETY)]
        public string IdPollinator { get; set; }

        /// <summary>
        /// Determina la raíz de las plantas de un cuartel.
        /// </summary>
        [Group(2, PhisicalDevice.WEB, 6)]
        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        public string IdRootstock { get; set; }



        /// <summary>
        /// Temporada a la que pertenece el cuartel.
        /// </summary>
        [Group(3, PhisicalDevice.WEB, 6)]
        [ReferenceSearch(EntityRelated.SEASON)]
        public string IdSeason { get; set; }



    }
}