using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input
{
    public class PreOrderInput : InputBaseName
    {
        
        public string OrderFolderId { get; set; }

        public string IdIngredient { get; set; }


        public PreOrderType PreOrderType { get; set; }


        public string[] BarracksId { get; set; }


    }

    public class PreOrderSwaggerInput 
    {
        [Required]
        public string Name { get; set; }

        
        public string OrderFolderId { get; set; }
        
        public string IdIngredient { get; set; }



        [Required]
        public PreOrderType PreOrderType { get; set; }


        [Required]
        public string[] BarracksId { get; set; }


    }




}