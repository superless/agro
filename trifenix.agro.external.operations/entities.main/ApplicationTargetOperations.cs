using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class ApplicationTargetOperations : IApplicationTargetOperations
    {
        private readonly IApplicationTargetRepository _repo;
        public ApplicationTargetOperations(IApplicationTargetRepository repo)
        {
            _repo = repo;
        }
        public async Task<ExtGetContainer<List<ApplicationTarget>>> GetAplicationsTarget()
        {
            try
            {
                var elements = await _repo.GetTargets().ToListAsync();

                return new ExtGetContainer<List<ApplicationTarget>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };

            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<ApplicationTarget>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtPostContainer<ApplicationTarget>> SaveEditApplicationTarget(string id, string name)
        {
            try
            {
                var appTarget = await _repo.GetTarget(id);
                if (appTarget == null)
                {
                    return new ExtPostErrorContainer<ApplicationTarget>
                    {
                        Message = $"No existe propósito de aplicación con id : {id}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }


                appTarget.Name = name;

                await _repo.CreateUpdateTargetApp(appTarget);

                return new ExtPostContainer<ApplicationTarget>
                {
                    Result = appTarget,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostContainer<ApplicationTarget>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };
                
            }

        }

        public async Task<ExtPostContainer<string>> SaveNewApplicationTarget(string name)
        {
            try
            {
                var idResult = await _repo.CreateUpdateTargetApp(new ApplicationTarget
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = name
                });
                return new ExtPostContainer<string>
                {
                    IdRelated = idResult,
                    Result = idResult,
                    MessageResult = ExtMessageResult.Ok
                };

                    
            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<string>
                {
                    InternalException = ex,
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error
                };
            }
        }
    }
}
