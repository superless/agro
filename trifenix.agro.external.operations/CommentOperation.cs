using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities
{

    public class CommentOperation : MainOperation<Comment, CommentInput>, IGenericOperation<Comment, CommentInput> {

        public CommentOperation(IMainGenericDb<Comment> repo, IExistElement existElement, IAgroSearch<GeographyPoint> search, ICommonDbOperations<Comment> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

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