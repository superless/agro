using Cosmonaut;
using Cosmonaut.Extensions;
using Newtonsoft.Json;
using System;
using trifenix.agro.applicationsReference;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.model.enforcements.@base;

namespace trifenix.agro.console
{
    class Program
    {
        static void Main(string[] args)
        {



            AgroDbArguments argos = new AgroDbArguments
            {
                NameDb = "agrodb",
                EndPointUrl = "https://localhost:8081",
                PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            };


            var StoreSettings = new CosmosStoreSettings(argos.NameDb, argos.EndPointUrl, argos.PrimaryKey);
            
            var Store = new CosmosStore<DocumentBase>(StoreSettings);


            var resultTask = Store.Query("Select * from c", null, new Microsoft.Azure.Documents.Client.FeedOptions
            {
                EnableCrossPartitionQuery = true
            }).ToListAsync();



           

            Console.WriteLine(JsonConvert.SerializeObject(resultTask));
            Console.ReadLine();
        }



    }

   
}
