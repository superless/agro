using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMainDb<Comment> _db;
        public CommentRepository(AgroDbArguments dbArguments)
        {
            _db = new MainDb<Comment>(dbArguments);
        }

        public async Task<string> CreateUpdateComment(Comment comment)
        {
            return await _db.CreateUpdate(comment);
        }

        public async Task<Comment> GetComment(string id)
        {
            return await _db.GetEntity(id);
        }
    }
}
