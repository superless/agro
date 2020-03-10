using System;

namespace trifenix.agro.model.external.Input
{
    public class CommentInput : InputBase {
        public string Commentary { get; set; }

       

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }
    }

    public class CommentSwaggerInput 
    {
        public string Commentary { get; set; }

      

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }
    }


}