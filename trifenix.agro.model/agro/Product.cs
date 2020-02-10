using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro {

    /// <summary>
    /// Producto Quimico usado por las órdenes
    /// </summary>
    [SharedCosmosCollection("agro", "Product")]
    public class Product : DocumentBase, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador en base de datos
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Nombre Comercial
        /// </summary>
        public string CommercialName { get; set; }

        /// <summary>
        /// Ingrediente Activo
        /// </summary>
        public Ingredient ActiveIngredient { get; set; }


        /// <summary>
        /// Marca del producto
        /// </summary>
        public string Brand { get; set; }

        private List<Doses> _doses;

        /// <summary>
        /// Recetas que aplican para el producto
        /// </summary>
        public List<Doses> Doses
        {
            get
            {
                _doses = _doses ?? new List<Doses>();
                return _doses;
            }
            set { _doses = value; }
        }



        private List<string> _idsCertifiedEntities;

        /// <summary>
        /// Ids de las entidades certificadoras, dentro de las dosis
        /// </summary>
        public List<string> IdsCertifiedEntities
        {
            get
            {
                _idsCertifiedEntities = _idsCertifiedEntities ?? new List<string>();
                return _idsCertifiedEntities;
            }
            set { _idsCertifiedEntities = value; }
        }


        private List<string> _idsTargets;


        /// <summary>
        /// Ids de las enfermedades dentro de las dosis
        /// </summary>
        public List<string> IdsTargets
        {
            get
            {
                _idsTargets = _idsTargets ?? new List<string>();
                return _idsTargets;
            }
            set { _idsTargets = value; }
        }

        private List<string> _idVarieties;


        /// <summary>
        /// variedades dentro de las dosis
        /// </summary>
        public List<string> IdVarieties
        {
            get
            {
                _idVarieties = _idVarieties ?? new List<string>();
                return _idVarieties;
            }
            set { _idVarieties = value; }
        }

        private List<string> __idsSpecies;


        /// <summary>
        /// especies dentro de las dosis
        /// </summary>
        public List<string> IdsSpecies
        {
            get
            {
                __idsSpecies = __idsSpecies ?? new List<string>();
                return __idsSpecies;
            }
            set { __idsSpecies = value; }
        }

        public MeasureType MeasureType { get; set; }

        public int QuantityByContainer {get ; set;}

        public KindOfProductContainer KindOfContainer { get; set; }

    }
}