using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.model.external.output
{
    public class OrderResult
    {
        public long Total { get; set; }

        public OutPutApplicationOrder[] Orders { get; set; }
    }
}
