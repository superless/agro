using Cosmonaut;
using Cosmonaut.Response;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using trifenix.agro.external.operations;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.helper;
using trifenix.connect.db;

namespace trifenix.agro.console
{

    class Program {

        

        static async Task Main(string[] args) {

            var dict = new Dictionary<int, string> { { 1, "hello" }, { 2, "hello" } };

            Console.WriteLine(JsonConvert.SerializeObject(dict));
        }

    }

}