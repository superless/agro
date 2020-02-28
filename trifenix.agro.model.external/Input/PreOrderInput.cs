using System.Collections.Generic;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class PreOrderInput : InputBaseName
    {
        
        public string OrderFolderId { get; set; }

        public string IdIngredient { get; set; }


        public PreOrderType PreOrderType { get; set; }


        private List<string> _barracksId;

        public List<string> BarracksId
        {
            get
            {
                _barracksId = _barracksId ?? new List<string>();
                return _barracksId;
            }
            set { _barracksId = value; }
        }

    }




}