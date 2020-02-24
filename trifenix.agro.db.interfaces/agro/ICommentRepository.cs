using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface ICommentRepository
    {
        Task<string> CreateUpdateComment(Comment comment);

        Task<Comment> GetComment(string id);


    }
}
