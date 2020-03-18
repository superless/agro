using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace trifenix.agro.cosmosdbinitializer.interfaces {
    public interface ICosmosDbInitializer {

        Task<JsonResult> MapJsonToDB(dynamic json);

    }

}