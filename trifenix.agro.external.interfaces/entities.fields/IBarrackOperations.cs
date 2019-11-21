using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.fields
{
    public interface IBarrackOperations
    {
        Task<ExtPostContainer<string>> SaveNewBarrack(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock);


        Task<ExtPostContainer<Barrack>> SaveEditBarrack(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock);

        Task<ExtGetContainer<List<Barrack>>> GetBarracks();

        Task<ExtGetContainer<Barrack>> GetBarrack(string id);

        
    }
}
