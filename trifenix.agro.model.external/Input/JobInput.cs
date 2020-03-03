using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class JobInput : InputBaseName
    {


    }

    public class JobSwaggerInput {


        public JobSwaggerInput()
        {
            var f = new JobSwaggerInput();

            var typef = f.GetType();

            var props = typef.GetProperties();

            var propsEnums = props.Where(prop => prop.GetCustomAttributes(typeof(CheckEnumeradorAttribute), true) != null);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(typeof(CheckEnumeradorAttribute), true) != null)
                {
                    var attr = (CheckEnumeradorAttribute)prop.GetCustomAttributes(typeof(CheckEnumeradorAttribute), true)[0];
                    var entidad = attr.entidadRelacionada;
                    prop.SetValue(f, entidad.ToString());

                    
                }
            }


        }

        [Required]
        [CheckEnumerador(PropertyRelated.GENERIC_CODE)]
        public string Name { get; set; }


  
        public string Valor { get; set; }



    }


    public class CheckEnumeradorAttribute : Attribute {
        public readonly PropertyRelated entidadRelacionada;

        public CheckEnumeradorAttribute(PropertyRelated entidadRelacionada)
        {
            this.entidadRelacionada = entidadRelacionada;
        }
    
    }
}
