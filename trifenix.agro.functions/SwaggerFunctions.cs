using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using System.Net.Http;
using AzureFunctions.Extensions.Swashbuckle;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace trifenix.agro.functions
{
    public static class SwaggerFunctions
    {

        [SwaggerIgnore]

        [FunctionName("Swagger")]
        
        public async static Task<IActionResult> Swagger(

            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")]

            HttpRequestMessage req,

            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)

        {

            var result = swashBuckleClient.CreateSwaggerDocumentResponse(req);



            var getResult =  Task.FromResult(result);

            var js = await getResult.Result.Content.ReadAsStringAsync();

            var swaggerElement = JObject.Parse(js);



            swaggerElement["info"]["version"] = "v1";
            swaggerElement["info"]["title"] = "AgroFenix";
            swaggerElement["info"]["description"] = "API Oficial de AgroFenix";



            var strJs = swaggerElement.ToString();

            return new JsonResult(swaggerElement);

        }



        [SwaggerIgnore]

        [FunctionName("SwaggerUi")]

        public static Task<HttpResponseMessage> SwaggerUi(

            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")]

            HttpRequestMessage req,

            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)

        {
            

            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));

        }

    }
}
