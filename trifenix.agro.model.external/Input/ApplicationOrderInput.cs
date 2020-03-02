using System;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{

    public class ApplicationOrderInput : InputBaseName
    {
        public OrderType OrderType { get; set; }

        
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Wetting { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

        public string[] IdsPhenologicalPreOrder { get; set; }

        public BarrackOrderInstance[] Barracks { get; set; }

    }


}