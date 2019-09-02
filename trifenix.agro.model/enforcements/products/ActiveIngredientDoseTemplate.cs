using System;

namespace trifenix.agro.model.enforcements.products
{
    /// <summary>
    /// Template Doses by an active ingredient
    /// </summary>
    public class ActiveIngredientDoseTemplate
    {
        public string Id { get; set; }

        /// <summary>
        /// Name of the template
        /// </summary>
        public string TemplateName { get; set; }


        /// <summary>
        /// Dose in cc or kg by 100Lts of water
        /// </summary>
        public int DoseBy100 { get; set; }


        /// <summary>
        /// Date Creation of the template
        /// </summary>
        public DateTime DateCreation { get; set; }


        /// <summary>
        /// Measure type on template
        /// </summary>
        public MeausereType MeausereType { get; set; }


        /// <summary>
        /// days out of the field for security reasons.
        /// </summary>
        public Cadence Cadence { get; set; }


        /// <summary>
        /// Active Ingredient
        /// </summary>
        public ActiveIngredient ActiveIngredient { get; set; }


    }

}
