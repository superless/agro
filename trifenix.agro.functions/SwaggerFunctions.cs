using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace trifenix.agro.functions
{
    public static class SwaggerFunctions
    {
        [FunctionName("Swagger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            var result = swashBuckleClient.CreateSwaggerDocumentResponse(req);
            var getResult = Task.FromResult(result);
            var js = await getResult.Result.Content.ReadAsStringAsync();
            var swaggerElement = JObject.Parse(js);
            swaggerElement["info"]["version"] = "v1";
            swaggerElement["info"]["title"] = "AgroFenix";
            swaggerElement["info"]["description"] = "API Oficial de AgroFenix";
            return new JsonResult(swaggerElement);
        }

        [FunctionName("SwaggerUi")]
        public static Task<HttpResponseMessage> Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        }

    }
}