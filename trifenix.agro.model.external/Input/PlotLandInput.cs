using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;

namespace trifenix.agro.model.external.Input {
    public class PlotLandInput : InputBase {
        [Required, ReferenceAttribute(typeof(Sector))]
        public string IdSector { get; set; }

        [Required, Unique]
        public string Name { get; set; }

    }

    public class PlotLandSwaggerInput  {

        /// <summary>
        /// Nombre de Parcela
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Id del sector al que pertenece la parcela.
        /// </summary>
        [Required]
        public string IdSector { get; set; }

    }

}