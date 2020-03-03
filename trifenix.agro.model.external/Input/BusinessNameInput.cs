using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class BusinessNameInput : InputBaseName
    {
        public string Email { get; set; }
        public string Rut { get; set; }

        public string WebPage { get; set; }
        public string Giro { get; set; }
        public string Phone { get; set; }
    }

    public class BusinessNameSwaggerInput {


        [Required]
        public string Name { get; set; }


        [Required]
        public string Email { get; set; }

        [Required]
        public string Rut { get; set; }


        public string WebPage { get; set; }


        public string Giro { get; set; }
        public string Phone { get; set; }

    }
}
