using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{

    public class CommentOperation<T> : MainOperation<Comment, CommentInput, T>, IGenericOperation<Comment, CommentInput> {

        public CommentOperation(IMainGenericDb<Comment> repo, IAgroSearch<T> search, ICommonDbOperations<Comment> commonDb, IValidatorAttributes<CommentInput, Comment> validators) : base(repo, search, commonDb, validators) { }

        public async Task<ExtPostContainer<string>> Save(Comment comment) {
            await repo.CreateUpdate(comment);
            search.AddDocument(comment);
            return new ExtPostContainer<string> {
                IdRelated = comment.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task Remove(string id) { }

        public async Task<ExtPostContainer<string>> SaveInput(CommentInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            //var validaComment = await ValidaComment(input);
            var comment = new Comment {
                Id = id,
                Commentary = input.Commentary,
                Created = DateTime.Now,
                EntityId = input.EntityId,
                EntityIndex = input.EntityIndex,
                IdUser = input.IdUser
            };
            if (!isBatch)
                return await Save(comment);
            await repo.CreateEntityContainer(comment);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}