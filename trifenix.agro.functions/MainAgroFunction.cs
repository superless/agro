using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

using trifenix.agro.db.model.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.helper;
using trifenix.agro.functions.Helper;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.model.external;
using trifenix.agro.search.model;
using trifenix.agro.email.operations;
using trifenix.agro.db.model.agro.core;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using trifenix.agro.swagger.model.input;
using System.Net;
using trifenix.agro.enums;

namespace trifenix.agro.functions {
    public static class MainAgroFunction {




        //#region v2/executions
        //[SwaggerIgnore]
        //[FunctionName("Executions")]
        //public static async Task<IActionResult> Executions([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/executions/{idExecution?}/{abbSpecie?}/{status?}/{textToSearch?}/{asc?}/{totalByPage?}/{page?}")] HttpRequest req, string idExecution, string abbSpecie, string status, string textToSearch, string asc, int? totalByPage, int? page, ILogger log) {
        //    ClaimsPrincipal claims = await Auth.Validate(req);

        //    if (claims == null)
        //        return new UnauthorizedResult();

        //    var manager = await ContainerMethods.AgroManager(claims);

        //    ExtGetContainer<ExecutionOrder> result = null;


        //    switch (req.Method.ToLower()) {
        //        case "get":
        //            if (!string.IsNullOrWhiteSpace(idExecution) && !idExecution.ToLower().Equals("all")) {
        //                result = await manager.ExecutionOrders.GetExecutionOrder(idExecution);
        //                return ContainerMethods.GetJsonGetContainer(result, log);
        //            }
        //            break;
        //        case "post":
        //            return await ContainerMethods.ApiPostOperations<string>(req.Body, log, async (db, model) => {
        //                string idOrder = (string)model["idOrder"];
        //                string idUserApplicator = (string)model["idUserApplicator"];
        //                string idNebulizer = (string)model["idNebulizer"];
        //                string[] idProduct = JsonConvert.DeserializeObject<string[]>(((object)model["idProduct"]).ToString());
        //                double[] quantityByHectare = JsonConvert.DeserializeObject<double[]>(((object)model["quantityByHectare"]).ToString());
        //                string idTractor = (string)model["idTractor"];
        //                string commentary = (string)model["commentary"];
        //                string executionName = (string)model["executionName"];
        //                return await db.ExecutionOrders.SaveNewExecutionOrder(idOrder, executionName, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor, commentary);
        //            }, claims);
        //        case "put":
        //            return await ContainerMethods.ApiPostOperations<ExecutionOrder>(req.Body, log, async (db, model) => {
        //                string idOrder = (string)model["idOrder"];
        //                string idUserApplicator = (string)model["idUserApplicator"];
        //                string idNebulizer = (string)model["idNebulizer"];
        //                string[] idProduct = JsonConvert.DeserializeObject<string[]>(((object)model["idProduct"]).ToString());
        //                double[] quantityByHectare = JsonConvert.DeserializeObject<double[]>(((object)model["quantityByHectare"]).ToString());
        //                string idTractor = (string)model["idTractor"];
        //                string executionName = (string)model["executionName"];
        //                return await db.ExecutionOrders.SaveEditExecutionOrder(idExecution, idOrder, executionName, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor);
        //            }, claims);
        //    }
        //    if (!string.IsNullOrWhiteSpace(textToSearch) && textToSearch.Equals("*"))
        //        textToSearch = null;
        //    if (!string.IsNullOrWhiteSpace(abbSpecie) && abbSpecie.ToLower().Equals("all"))
        //        abbSpecie = null;
        //    else abbSpecie = abbSpecie.ToUpper();
        //    int? statusToFilter = null;
        //    if (!string.IsNullOrWhiteSpace(status) && !status.ToLower().Equals("all")) {
        //        if (int.TryParse(status, out int outResult)) statusToFilter = outResult;
        //        else return new BadRequestResult();
        //    }
        //    bool? order = null;
        //    if (!string.IsNullOrWhiteSpace(asc) && !asc.ToLower().Equals("not_order")) {
        //        if (asc.ToLower().Equals("asc")) order = true;
        //        else if (asc.ToLower().Equals("desc")) order = false;
        //        else return new BadRequestResult();
        //    }
        //    var resultGetByPageAll = manager.ExecutionOrders.GetPaginatedExecutions(textToSearch, abbSpecie, statusToFilter, page, totalByPage, order);
        //    return ContainerMethods.GetJsonGetContainer(resultGetByPageAll, log);
        //}
        //#endregion

        //#region v2/Executions_ChangeStatus
        //[FunctionName("Execution_ChangeStatus")]
        //public static async Task<IActionResult> Execution_ChangeStatus([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/execution/changeStatus/{idExecution}")] HttpRequest req, string idExecution, ILogger log) {
        //    ClaimsPrincipal claims = await Auth.Validate(req);
        //    if (claims == null)
        //        return new UnauthorizedResult();

        //    var manager = await ContainerMethods.AgroManager(claims);

        //    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
        //        var type = (string)model["type"];
        //        var value = (int)model["value"];
        //        var commentary = (string)model["commentary"];
        //        return await manager.ExecutionOrders.SetStatus(idExecution, type, value, commentary);
        //    }, claims);
        //}
        //#endregion

        //#region v2/executionsAddCommentary
        //[FunctionName("ExecutionsAddCommentary")]
        //public static async Task<IActionResult> ExecutionsAddCommentary([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/execution/add_commentary/{idExecution}")] HttpRequest req, string idExecution, ILogger log) {
        //    ClaimsPrincipal claims = await Auth.Validate(req);
        //    if (claims == null)
        //        return new UnauthorizedResult();
        //    return await ContainerMethods.ApiPostOperations<ExecutionOrder>(req.Body, log, async (db, model) => {
        //        string commentary = (string)model["commentary"];
        //        return await db.ExecutionOrders.AddCommentaryToExecutionOrder(idExecution, commentary);
        //    }, claims);
        //}
        //#endregion



        #region Login

        /// <summary>
        /// Obtiene el token de usuario, al introducir el usuario y contraseña.
        /// </summary>
        /// <param name="req">elemento request</param>
        /// <param name="log">elemento de logs</param>
        /// <returns></returns>
        
        #endregion

        [FunctionName("DebugRoute")]
        public static async Task<IActionResult> DebugRoute([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/debugroute/{id?}")] HttpRequest req, ILogger log, string id){
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic result = JsonConvert.DeserializeObject(requestBody);
            return result;
        }


    }
}