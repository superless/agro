using System;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.mdm.entity_model
{

    /// <summary>
    /// Clase generada automáticamente desde una entidad de base de datos o cualquier clase.
    /// Un entitySearch es una estructura diseñada para las busquedas.
    /// </summary>
    /// <typeparam name="T">Tipo de dato Geo</typeparam>
    public class EntityBaseSearch<T> : IEntitySearch<T>
    {

        /// <summary>
        /// identificador de un entitySearch
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Fecha de creación del elemento.
        /// </summary>
        public DateTime created { get; set; }

        /// <summary>
        /// índice de la entidad, la que identifica como elemento.
        /// un índice viene de un diccionario que enlaza con la clase que corresponda 
        /// a través de atributos.
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// Todas las propiedades booleanas, quedarán en esta colección.
        /// </summary>
        public IBoolProperty[] bl { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo double
        /// </summary>
        public IDblProperty[] dbl { get; set; }

        /// <summary>
        /// colección de propiedades de tipo fecha.
        /// </summary>
        public IDtProperty[] dt { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo enumeración.
        /// </summary>
        public IEnumProperty[] enm { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo num32.
        /// </summary>
        public INum32Property[] num32 { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo num64.
        /// </summary>
        public INum64Property[] num64 { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo relación.
        /// </summary>
        public IRelatedId[] rel { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo string.
        /// </summary>
        public IStrProperty[] str { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo suggest.
        /// </summary>
        public IStrProperty[] sug { get; set; }

        /// <summary>
        /// Colección de propiedades de tipo geo,
        /// el tipo Geo esta determinado por el tipo que se asigne al objeto.
        /// </summary>
        public IProperty<T>[] geo { get; set; }

        public string hm { get; set; }

        public string hh { get; set; }

    }



}