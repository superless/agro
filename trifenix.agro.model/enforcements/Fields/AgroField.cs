using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.model.enforcements.@base;

namespace trifenix.agro.db.model.enforcements.Fields
{

    [SharedCosmosCollection("agro", "AgroField")]
    public class AgroField : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public double Hectares { get; set; }

        private List<AgroVariety> _varieties;

        public List<AgroVariety> Varieties
        {
            get {
                _varieties = _varieties ?? new List<AgroVariety>();
                return _varieties;

            }
            set { _varieties = value; }
        }

        public string Precessor { get; set; }

        public AgroYear Season { get; set; }



    }
}
