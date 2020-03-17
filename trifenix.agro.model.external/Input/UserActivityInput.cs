using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input {

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