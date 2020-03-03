using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class PlotLandInput : InputBaseName
    {
       

        public string IdSector { get; set; }

    }

    public class PlotLandSwaggerInput 
    {

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
