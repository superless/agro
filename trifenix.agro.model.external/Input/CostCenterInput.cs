using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class CostCenterInput : InputBaseName
    {
       
        public string IdBusinessName { get; set; }

    }

    public class CostCenterSwaggerInput {

        [Required]
        public string Name { get; set; }


        [Required]
        public string IdBusinessName { get; set; }
    }
}
