using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;

namespace trifenix.agro.db.model.enforcements.products
{
    /// <summary>
    /// Template Doses by an active ingredient
    /// </summary>

    [SharedCosmosCollection("agro", "DoseTemplate")]
    public class DoseTemplate : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

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
        private List<Cadence> _cadences;

        public List<Cadence> Cadences
        {
            get {
                _cadences = _cadences ?? new List<Cadence>();
                return _cadences; }
            set { _cadences = value; }
        }



        public ApplicationMethod ApplicationMethod { get; set; }

        public ActiveIngredient ActiveIngredient { get; set; }



    }

}
