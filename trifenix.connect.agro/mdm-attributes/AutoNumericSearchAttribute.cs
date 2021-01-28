using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;

using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Atributo utilizado para determinar si un campo es autonumérico.
    /// </summary>
    public class AutoNumericSearchAttribute : AutoNumericDependantAttribute
    {

        /// <summary>
        /// Indica si una propiedad es autonumérica.
        /// </summary>
        /// <param name="index">Índice de la propiedad</param>
        public AutoNumericSearchAttribute(StringRelated index)
        {
            Index = (int)index;
            KindIndex = (int)KindProperty.STR;
        }

        /// <summary>
        /// Si un autonumérico depende de otra entidad se podrá indicar en el constructor,
        /// dependerán de otra entidad cuando la generación de la secuencia depende de otra entidad,
        /// por ejemplo un listado de producto que tiene un autonumérico que depende de una factura, todas las facturas empezarían desde cero.
        /// </summary>
        /// <param name="index">Índice de la propiedad numérica que será autonumérica</param>
        /// <param name="dependant">Índice de la entidad de la que depende</param>
        public AutoNumericSearchAttribute(StringRelated index, EntityRelated dependant): this(index)
        {
            Dependant = (int?)dependant;
            
        }
        

        

    }


}