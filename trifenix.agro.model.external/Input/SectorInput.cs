using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace trifenix.agro.model.external.Input
{
    public class SectorInput : InputBaseName
    {


    }



    /// <summary>
    /// Para ingresar un Sector debe determinar solo el nombre.
    /// </summary>
    public class SectorSwaggerInput
    {
        

        [Required]
        public string Name { get; set; }


    }

    
}
