using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.model.external;
using Cosmonaut.Extensions;
using trifenix.agro.db.interfaces.agro;
using System.Linq;

namespace trifenix.agro.external.operations.entities.main
{
    public class SeasonOperations : ISeasonOperations
    {

        private readonly ISeasonRepository _repo;

        public SeasonOperations(ISeasonRepository repo)
        {
            _repo = repo;
        }
        public async Task<ExtGetContainer<List<Season>>> GetSeasons()
        {
            try
            {
                var elements = await _repo.GetSeasons().ToListAsync();

                return new ExtGetContainer<List<Season>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };

            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<Season>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtPostContainer<Season>> SaveEditSeason(string id, DateTime init, DateTime end, bool current)
        {
            try
            {
                var season = await _repo.GetSeason(id);
                if (season == null)
                {
                    return new ExtPostErrorContainer<Season>
                    {
                        Message = $"No existe temporada con id : {id}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }


                season.Current = current;
                season.Start = init;
                season.End = end;


                await _repo.CreateUpdateSeason(season);

                return new ExtPostContainer<Season>
                {
                    Result = season,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostContainer<Season>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };

            }
        }

        public async Task<ExtPostContainer<string>> SaveNewSeason(DateTime init, DateTime end)
        {
            try
            {
                var idResult = await _repo.CreateUpdateSeason(new Season
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Start = init,
                    End = end
                    
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
