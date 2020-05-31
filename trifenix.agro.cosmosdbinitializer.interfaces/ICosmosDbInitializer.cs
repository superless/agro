using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace trifenix.agro.cosmosdbinitializer.interfaces {
    public interface ICosmosDbInitializer {

        Task<JsonResult> MapJsonToDB(JObject json);

    }

}