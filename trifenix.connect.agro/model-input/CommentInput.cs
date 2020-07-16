
using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.model;

namespace trifenix.connect.agro.model_input
{
    public class CommentInput : InputBase {

        [Required]
        public string Commentary { get; set; }

        [Required, Reference(typeof(User))]
        public string IdUser { get; set; }

        [Required]
        public int EntityIndex { get; set; }

        [Required]
        public string EntityId { get; set; }
    }

    public class CommentSwaggerInput  {
        public string Commentary { get; set; }

        public string IdUser { get; set; }

        public int EntityIndex { get; set; }

        public string EntityId { get; set; }
    }

}