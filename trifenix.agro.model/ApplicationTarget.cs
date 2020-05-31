using Cosmonaut;
using Cosmonaut.Attributes;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {


    /// <summary>
    /// Objetivo de la aplicación, 
    /// Cual es el objetivo de la aplicación de una órden.
    /// </summary>
    [SharedCosmosCollection("agro", "ApplicationTarget")]
    [ReferenceSearchHeader(EntityRelated.TARGET, PathName = "targets", Kind = EntityKind.ENTITY)]
    public class ApplicationTarget : DocumentBaseName<long>, ISharedCosmosEntity {


        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// El identificador de cliente el que será mostrado en el formulario y la vista.
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        /// <summary>
        /// Nombre del objetivo de la aplicación
        /// </summary>
        [Group(0, Device.WEB, proportion: 6)]
        [Required]
        [Unique]

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        /// <summary>
        /// Abreviación del objetivo.
        /// </summary>
        [Group(0, Device.WEB, proportion:3)]
        [Unique]        
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}