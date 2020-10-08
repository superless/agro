using Microsoft.Azure.Search;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.search.model
{

    /// <summary>
    /// Implementación de azure search de modelo de entidades 
    /// de fenix mdm.
    /// Esta implementación añade metadata que permitirá optimizar las busquedas 
    /// según el tipo de propiedad.
    /// esta entidad es un contenedor de valores de una clase. 
    /// donde con un diccionario es posible identificar los valores que encuentran en estos contenedores.
    /// a través de un modelo de atributos, originamos una metadata que permite poder crear 
    /// una estructura de elementos indexados para lecturas de alta velocidad
    /// y que permite crear una arquitectura de componentes tanto para web o móvil, que 
    /// consulten a este modelo sin necesidad de conocer el negocio en el que será implementado.
    /// esta clase busca mantener arrays por cada tipo de dato para poder mejorar las busquedas.
    /// ocupar poco espacio, por eso los nombres cortos.
    /// sin embargo, incluso al guardar una entidad vacia, se generarán los arrays vacios, que ocupan espacio.
    /// por otro lado, los valores nulos no son incluidos en este modelo.
    /// si se identifica una entidad con el índice de una clase, las propiedades que no tengan valores en este modelo
    /// se entenderán como nulas.
    /// </summary>
    public class EntitySearch
    {

        /// <summary>
        /// Identificador de la entidad. 
        /// aquí asignarémos por regla general un guid que identifique a la entidad
        /// </summary>
        [Key]
        [IsFilterable]
        [JsonProperty("id")]
        public string id { get; set; }


        /// <summary>
        /// índice que será usado con un diccionario para identificar el elemento.
        /// por ejemplo, 
        /// si un elemento es una persona, el índice será igual a 1, que es el índice que el diccionario tiene para persona.
        /// </summary>
        [IsFilterable, IsFacetable]
        [JsonProperty("index")]
        public int index { get; set; }


        /// <summary>
        /// Fecha de creación, esto no está siendo utilizado aún,
        /// se tiene la idea de usarlo como ttl, pero hay que investigar acerca de la 
        /// metadata propia de azure, la cual podría tener un timestamp para hacer operaciones de este tipo.
        /// </summary>
        [IsSortable]
        [JsonProperty("created")]
        public DateTime created { get; set; }


        /// <summary>
        /// propiedad que referencia otras entidades, 
        /// estas entidades son identificados por su índice y su valor.
        /// esta propiedad relaciona entidades, como una profesor y sus salas o
        /// una especie de una fruta a sus variedades.
        /// esta propiedad implementa facet por lo que permite agrupación de busquedas.
        /// </summary>
        [JsonProperty("rel")]
        public RelatedId[] rel { get; set; }


        

        /// <summary>
        /// todas las propiedades que sean asignadas como suggest,
        /// serán utilizadas para autocompletado o sugerencias de busqueda
        /// esto permite mejorar las busquedas, pero también aumentará el tamaño de la base de datos.
        /// </summary>
        [JsonProperty("sug")]
        public StrProperty[] sug { get; set; }


        /// <summary>
        /// colección de valores de tipo cadena de texto.
        /// </summary>
        [JsonProperty("str")]
        public StrProperty[] str { get; set; }


        /// <summary>
        /// colección de valores de tipo enumeración,
        /// este tipo de propiedad es de tipo facetable por tanto se puede agrupar por valor de la enumeración.
        /// </summary>
        [JsonProperty("enm")]
        public EnumProperty[] enm { get; set; }


        /// <summary>
        /// colección de propiedades de tipo entero.
        /// </summary>
        [JsonProperty("num32")]
        public Num32Property[] num32 { get; set; }

        /// <summary>
        /// colección de propiedades de tipo long.
        /// </summary>
        [JsonProperty("num64")]
        public Num64Property[] num64 { get; set; }


        /// <summary>
        /// colección de propiedades de tipo doble.
        /// </summary>
        [JsonProperty("dbl")]
        public DblProperty[] dbl { get; set; }


        /// <summary>
        /// colección de propiedades de tipo fecha.
        /// </summary>
        [JsonProperty("dt")]
        public DtProperty[] dt { get; set; }


        /// <summary>
        /// colección de propiedades de tipo geo.
        /// </summary>
        [JsonProperty("geo")]
        public GeoProperty[] geo { get; set; }


        /// <summary>
        /// colección de propiedades de tipo bool.
        /// </summary>
        [JsonProperty("bl")]
        public BoolProperty[] bl { get; set; }

    }
}
