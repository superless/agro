using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.connect.interfaces.web
{
    public interface ICosmosDbInitializer
    {
        Task<JsonResult> MapJsonToDB(JObject json);
    }
}
