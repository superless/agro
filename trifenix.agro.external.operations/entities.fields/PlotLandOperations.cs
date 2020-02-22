using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.fields
{
    public class PlotLandOperations : IPlotLandOperations
    {

        private readonly IPlotLandRepository _repo;
        private readonly ISectorRepository _repoSector;
        private readonly string _idSeason;
        private readonly ICommonDbOperations<PlotLand> _commonDb;
        public PlotLandOperations(IPlotLandRepository repoPlotLand, ISectorRepository repoSector, ICommonDbOperations<PlotLand> commonDb, string idSeason  )
        {
            _repo = repoPlotLand;
            _repoSector = repoSector;
            _idSeason = idSeason;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<PlotLand>> GetPlotLand(string id)
        {
            var plotLand = await _repo.GetPlotLand(id);
            return OperationHelper.GetElement(plotLand);
        }

        public async Task<ExtGetContainer<List<PlotLand>>> GetPlotLands()
        {
            var queryPlotands = _repo.GetPlotLands();
            var plotLands = await _commonDb.TolistAsync(queryPlotands);
            return OperationHelper.GetElements(plotLands);

        }

        public async Task<ExtPostContainer<PlotLand>> SaveEditPlotLand(string id, string name, string idSector)
        {
            var sector = await _repoSector.GetSector(idSector);
            if (sector == null)
            {
                return OperationHelper.PostNotFoundElementException<PlotLand>($"no existe sector con id {idSector}", idSector);
            }

            var element = await _repo.GetPlotLand(id);

            return await OperationHelper.EditElement(_commonDb, _repo.GetPlotLands(),
                id,
                element,
                s => {
                    s.Name = name;
                    s.SeasonId = _idSeason;
                    s.Sector = sector;
                    return s;
                },
                _repo.CreateUpdateSector,
                 $"No existe Parcela con id :{id}",
                s => s.Name.Equals(name) && name!=element.Name,
                $"Ya existe parcela con nombre: {name}"
            );
        }


        
        public async Task<ExtPostContainer<string>> SaveNewPlotLand(string name, string idSector)
        {
            var sector = await _repoSector.GetSector(idSector);
            if (sector == null)
            {
                return OperationHelper.PostNotFoundElementException<string>($"no existe sector con id {idSector}", idSector);
            }
            return await OperationHelper.CreateElement(_commonDb,_repo.GetPlotLands(),
                async s => await _repo.CreateUpdateSector(new PlotLand
                {
                    Id = s,
                    Name = name,
                    SeasonId = _idSeason,
                    Sector = sector
                }),
                s => s.Name.Equals(name),
                $"Ya existe parcela con nombre: {name}"
            );

        }
    }
}
