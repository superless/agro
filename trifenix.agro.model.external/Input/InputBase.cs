using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.model.external.Input
{
    public abstract class InputBase
    {
        public string Id { get; set; }

    }

    public abstract class InputBaseName : InputBase
    {
        
        public string Name { get; set; }

        

    }
}
