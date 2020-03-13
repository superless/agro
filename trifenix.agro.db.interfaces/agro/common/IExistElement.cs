using System.Threading.Tasks;
namespace trifenix.agro.db.interfaces.agro.common {

    public interface IExistElement {
        Task<bool> ExistsById<T>(string id) where T : DocumentBase;
        Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase;


        Task<bool> ExistsCustom<T>(string query) where T : DocumentBase;

    }
}