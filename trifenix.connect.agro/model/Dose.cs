using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de las dosis
    /// </summary>
    [SharedCosmosCollection("agro", "Dose")]
    [ReferenceSearchHeader(EntityRelated.DOSES, Kind = EntityKind.CUSTOM_ENTITY)]
    public class Dose : DocumentLocal {

        /// <summary>
        /// Identificador de una dosis        
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Genera un correlativo, pero este depende de producto.
        /// por tanto por cada producto volverá la secuencia a 1.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE, EntityRelated.PRODUCT)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Última modificación de la dosis.
        /// </summary>
        [DateSearch(DateRelated.LAST_MODIFIED)]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Identificador de producto.
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        /// <summary>
        /// Variedades asignadas a la dosis.
        /// una dosis puede aplicar a más de un tipo de variedad.
        /// </summary>
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string[] IdVarieties { get; set; }


        /// <summary>
        /// Especies relacionadas a la dosis
        /// </summary>
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string[] IdSpecies { get; set; }


        /// <summary>
        /// Objetivos de la dosis.
        /// </summary>
        [ReferenceSearch(EntityRelated.TARGET)]
        public string[] IdsApplicationTarget { get; set; }

        /// <summary>
        /// Número de horas que se debe esperar para entrar al cuartel.
        /// </summary>
        [Num32Search(NumRelated.HOURS_TO_ENTRY)]
        public int HoursToReEntryToBarrack { get; set; }


        /// <summary>
        /// Días que se determinarán para la próxima aplicación.
        /// </summary>
        [Num32Search(NumRelated.DAYS_INTERVAL)]
        public int ApplicationDaysInterval { get; set; }


        /// <summary>
        /// Número de aplicaciones continuadas.
        /// </summary>
        [Num32Search(NumRelated.NUMBER_OF_SECQUENTIAL_APPLICATION)]
        public int NumberOfSequentialApplication { get; set; }


        /// <summary>
        /// Mojado recomendado.
        /// </summary>
        [Num32Search(NumRelated.WETTING_RECOMMENDED)]
        public int WettingRecommendedByHectares { get; set; }


        /// <summary>
        /// Cantidad mínima determinada por la dosis
        /// </summary>
        [DoubleSearch(DoubleRelated.QUANTITY_MIN)]
        public double DosesQuantityMin { get; set; }

        /// <summary>
        /// Cantidad máxima determinada por la dosis 
        /// </summary>
        [DoubleSearch(DoubleRelated.QUANTITY_MAX)]
        public double DosesQuantityMax { get; set; }


        /// <summary>
        /// Número  de días de espera antes de la cosecha.
        /// determinado por la etiqueta del producto.
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int? WaitingDaysLabel { get; set; }


        private List<WaitingHarvest> _waitingToHarvest;

        /// <summary>
        /// Dias para cosechar por entidad certificadora
        /// </summary>
        [ReferenceSearch(EntityRelated.WAITINGHARVEST, true)]
        public List<WaitingHarvest> WaitingToHarvest {
            get {
                _waitingToHarvest = _waitingToHarvest ?? new List<WaitingHarvest>();
                return _waitingToHarvest;
            }
            set { _waitingToHarvest = value; }
        }

        /// <summary>
        /// determina si la dosis es por defecto.
        /// si un producto no se le asignan dosis, siempre tendrá uno.
        /// </summary>
        [BoolSearch(BoolRelated.GENERIC_DEFAULT)]
        public bool Default { get; set; }

        /// <summary>
        /// una dosis puede ser desactivada, si se requiere eliminar de un producto y esta está asociada con una orden.
        /// </summary>
        [BoolSearch(BoolRelated.GENERIC_ACTIVE)]
        public bool Active { get; set; }

    }

}