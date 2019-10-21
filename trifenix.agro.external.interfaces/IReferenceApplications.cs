using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements;
using trifenix.agro.db.model.enforcements.ApplicationOrders;
using trifenix.agro.db.model.enforcements.Fields;
using trifenix.agro.db.model.enforcements.products;
using trifenix.agro.db.model.enforcements.stages;
using trifenix.agro.model.external;
using trifenix.agro.model.external.@base;
using trifenix.agro.model.external.Helper;

namespace trifenix.agro.external.interfaces
{
    public interface IReferenceApplications
    {

        //approved
        Task<ExtPostContainer<string>> SavePhenologicalEvent(string name, DateTime date);

        //approved
        Task<ExtPostContainer<string>> SaveApplicationPurpose(string name);


        Task<ExtPostContainer<string>> SaveSeason(DateTime init, DateTime end);

        Task<ExtGetContainer<bool>> CurrentSeasonExists();

        Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents();


        Task<ExtGetContainer<List<ApplicationPurpose>>> GetApplicationPurposes();

        Task<ExtPostContainer<string>> SaveActiveIngredientCategory(string name);


        Task<ExtGetContainer<List<ActiveIngredientCategory>>> GetCategories();

        Task<ExtPostContainer<string>> SaveActiveIngredient(string name, string idCategory);

        Task<ExtGetContainer<List<ActiveIngredient>>> GetIngredients();

        Task<ExtPostContainer<string>> SaveSpecie(string name, string abbreviation);

        Task<ExtPostContainer<string>> SaveVariety(string name, string abbreviation, string idSpecie);

        Task<ExtGetContainer<List<AgroSpecie>>> GetSpecies();

        Task<ExtGetContainer<List<AgroVariety>>> GetVarieties();

        Task<ExtPostContainer<string>> SaveField(string name, string abbreviation, double hectares, string[] varieties, string precessor = null);

        Task<ExtGetContainer<List<AgroField>>> GetFields();

        Task<ExtPostContainer<TaskIdentifier>> SavePhenologicalOrder(string name, string idPhenologicalEvent, string idApplicationPurpose, int duration, ExternalApplication[] applications);

        Task<ExtPostContainer<TaskIdentifier>> SaveDateOrder(string name, DateTime initDate, string idApplicationPurpose, int duration, ExternalApplication[] applications);

        Task<ExtPostContainer<TaskIdentifier>> SaveContinuedOrder(string name, string precessor, string idApplicationPurpose, int duration, ExternalApplication[] applications);
        Task<ExtGetContainer<ExtSeason>> GetCurrentSeason();


    }
}
