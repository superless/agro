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
using trifenix.agro.external.operations.helper;

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
            var elements = await _repo.GetSeasons().ToListAsync();
            return OperationHelper.GetElements(elements);
        }

        public async Task<ExtPostContainer<Season>> SaveEditSeason(string id, DateTime init, DateTime end, bool current)
        {
            var element = await _repo.GetSeason(id);

            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Start = init;
                    s.End = end;
                    s.Current = current;
                    return s;
                },
                _repo.CreateUpdateSeason,
                 $"No existe temporada con id : {id}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewSeason(DateTime init, DateTime end)
        {

            //TODO: validar que no se pueda sobreponer fechas.
            return await OperationHelper.CreateElement(_repo.GetSeasons(),
                async s => await _repo.CreateUpdateSeason(new Season
                {
                    Id = s,
                    Start = init,
                    End = end,
                    Current = true
                }),
                s => false,
                $""

            );
        }
    }
}
