using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.connect.agro_model_input
{
#if CONNECT
    public class LocalBase
    {
        public string Id { get; set; }
    }
#else

    using trifenix.connect.input;

    public abstract class LocalBase : InputBase
    {
       
    }

#endif
}
