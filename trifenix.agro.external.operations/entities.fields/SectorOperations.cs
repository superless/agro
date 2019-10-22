using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.fields
{
    public class SectorOperations : ISectorOperations
    {

        private readonly ISectorRepository _repo;
        private readonly string _idSeason;

        public SectorOperations(ISectorRepository repo, string idSeason)
        {
            _repo = repo;
            _idSeason = idSeason;
        }

        public async Task<ExtGetContainer<Sector>> GetSector(string id)
        {
            var sector = await _repo.GetSector(id);
            return OperationHelper.GetElement(sector);
        }

        public async Task<ExtGetContainer<List<Sector>>> GetSectors()
        {
            var sectors = await _repo.GetSectors().ToListAsync();
            return OperationHelper.GetElements(sectors);
        }

        public async Task<ExtPostContainer<Sector>> SaveEditSector(string id, string name)
        {
            var element = await _repo.GetSector(id);

            return await OperationHelper.EditElement(id,
                element,
                s => {
                    s.Name = name;
                    s.SeasonId = _idSeason;
                    return s;
                },
                _repo.CreateUpdateSector,
                 $"No existe Sector con id : {id}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewSector(string name)
        {
            return await OperationHelper.CreateElement(_repo.GetSectors(),
                async s => await _repo.CreateUpdateSector(new Sector
                {
                    Id = s,
                    Name = name,
                    SeasonId = _idSeason
                }),
                s => s.Name.Equals(name),
                $"ya existe sector con nombre {name} "

            );
        }
    }
}
