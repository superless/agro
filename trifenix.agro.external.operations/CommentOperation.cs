using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities
{
    public class CommentOperation : MainReadOperation<Comment>, IGenericOperation<Comment, CommentInput>
    {
        public CommentOperation(IMainGenericDb<Comment> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Comment> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        private async Task<string> ValidaComment(CommentInput input)
        {
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var exists = await existElement.ExistsById<Comment>(input.Id);
                if (!exists) return $"El comentario con id {input.Id} no existe";
            }

            var userExists = await existElement.ExistsById<User>(input.IdUser);
            if (!userExists) return $"usuario con id {input.IdUser} no existe en la base de datos";

            return string.Empty;

        }

        public async Task<ExtPostContainer<string>> Save(CommentInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");


            var validaComment = await ValidaComment(input);

            if (!string.IsNullOrWhiteSpace(validaComment)) throw new Exception(validaComment);

            var comment = new Comment
            {
                Id = id,
                Commentary = input.Commentary,
                Created = DateTime.Now,
                EntityId = input.EntityId,
                EntityIndex = input.EntityIndex,
                IdUser = input.IdUser
            };

            await repo.CreateUpdate(comment);

            search.AddElements(new List<CommentSearch>
            {
                new CommentSearch{ 
                    IdUser = input.IdUser,
                    Comment = input.Commentary,
                    Created = DateTime.Now,
                    EntityIndex = input.EntityIndex,
                    Id = id,
                    EntityId = input.EntityId
                }
            });


            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }
}
