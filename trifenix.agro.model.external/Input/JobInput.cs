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



        [Required]
        
        public string Name { get; set; }





    }

}
