using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;

namespace trifenix.agro.model.external.Input {
    public class CommentInput : InputBase {

        [Required]
        public string Commentary { get; set; }

        [Required, ReferenceAttribute(typeof(User))]
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