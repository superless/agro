using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

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


    public class ApplicationOrderSwaggerInput 
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public DateTime InitDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public double Wetting { get; set; }

        [Required]
        public DosesOrder[] DosesOrder { get; set; }


        public string[] IdsPhenologicalPreOrder { get; set; }

        [Required]
        public BarrackOrderInstance[] Barracks { get; set; }

    }


}