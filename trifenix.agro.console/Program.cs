using System;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.console
{
    class Program
    {
        static async Task Main(string[] args){

            var db = new AgroRepository(new AgroDbArguments
            {
                EndPointUrl= "https://localhost:8081",
                NameDb="agrodb",
                PrimaryKey= "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            });


            var orders = db.Orders.GetApplicationOrders();

            var ordersDb = await new CommonDbOperations<ApplicationOrder>().TolistAsync(orders);




            Console.WriteLine(ordersDb.Count);

        }
    }
}
