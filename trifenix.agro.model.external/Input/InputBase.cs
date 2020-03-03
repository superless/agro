using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace trifenix.agro.model.external.Input
{
    public abstract class InputBase
    {
        public string Id { get; set; }

        
    }

    public abstract class InputBaseName : InputBase
    {
        [Required]
        public string Name { get; set; }

        

    }
}
