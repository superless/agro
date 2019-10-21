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
    public class PhenologicalEventOperations : IPhenologicalOperations
    {
        private readonly IPhenologicalEventRepositoy _repo;

        public PhenologicalEventOperations(IPhenologicalEventRepositoy repo)
        {
            _repo = repo;
        }


        public async Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents()
        {
            try
            {
                var elements = await _repo.GetPhenologicalEvents().ToListAsync();

                return new ExtGetContainer<List<PhenologicalEvent>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception ex)
            {

                return new ExtGetErrorContainer<List<PhenologicalEvent>>
                {
                    ErrorMessage = ex.Message,
                    StatusResult = ExtGetDataResult.Error
                };
            }
        }

        public async Task<ExtPostContainer<PhenologicalEvent>> SaveEditPhenologicalEvent(string currentId, string name, DateTime startDate, DateTime endDate)
        {
            try
            {
                var phenological = await _repo.GetPhenologicalEvent(currentId);
                if (phenological == null)
                {
                    return new ExtPostErrorContainer<PhenologicalEvent>
                    {
                        Message = $"No existe evento fenológico con id : {currentId}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = currentId
                    };
                }
                await _repo.CreateUpdatePhenologicalEvent(phenological);

                return new ExtPostContainer<PhenologicalEvent>
                {
                    IdRelated = currentId,
                    Result = phenological,
                    MessageResult = ExtMessageResult.Ok
                };

            }
            catch (Exception ex)
            {
                return new ExtPostContainer<PhenologicalEvent>
                {
                    IdRelated = currentId,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveNewPhenologicalEvent(string name, DateTime startDate, DateTime endDate)
        {
            try
            {
                var idResult = await _repo.CreateUpdatePhenologicalEvent(new PhenologicalEvent {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = name,
                    InitDate = startDate,
                    EndDate = endDate
                });
                return new ExtPostContainer<string>
                {
                    IdRelated = idResult,
                    MessageResult = ExtMessageResult.Ok,
                    Result = idResult
                };
            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<string>
                {
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = ex

                };
            }
        }

        
    }
}
