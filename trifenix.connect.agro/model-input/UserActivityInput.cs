using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;

namespace trifenix.connect.agro_model_input
{

    public class UserActivityInput : InputBase {

        [Required]
        public UserActivityAction Action { get; set; }
        public DateTime Date { get; set; }
        
        [Required]
        public string EntityName { get; set; }
        
        [Required]
        public string EntityId { get; set; }

    }

}