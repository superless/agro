using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.model.external.Input {
    public class CostCenterInput : InputBaseName {

        [Required,ReferenceAttribute(typeof(BusinessName))]
        public string IdBusinessName { get; set; }

    }

    public class CostCenterSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdBusinessName { get; set; }

    }

}