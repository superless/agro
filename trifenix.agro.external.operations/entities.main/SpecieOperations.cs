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
    public class SpecieOperations : ISpecieOperations
    {
        private readonly ISpecieRepository _repo;

        public SpecieOperations(ISpecieRepository repo)
        {
            _repo = repo;
        }

        public async Task<ExtGetContainer<List<Specie>>> GetSpecies()
        {
            try
            {
                var elements = await _repo.GetSpecies().ToListAsync();

                return new ExtGetContainer<List<Specie>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };

            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<Specie>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtPostContainer<Specie>> SaveEditSpecie(string id, string name)
        {
            try
            {
                var specie = await _repo.GetSpecie(id);
                if (specie == null)
                {
                    return new ExtPostErrorContainer<Specie>
                    {
                        Message = $"No existe especie con id : {id}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }


                specie.Name = name;

                await _repo.CreateUpdateSpecie(specie);

                return new ExtPostContainer<Specie>
                {
                    Result = specie,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostContainer<Specie>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };

            }
        }

        public async Task<ExtPostContainer<string>> SaveNewSpecie(string name)
        {
            try
            {
                var idResult = await _repo.CreateUpdateSpecie(new Specie
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
