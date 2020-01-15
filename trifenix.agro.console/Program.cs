using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.search;
using trifenix.agro.search.model;

namespace trifenix.agro.console
{
    class Program
    {
        static async Task Main(string[] args){

            //var db = new AgroRepository(new AgroDbArguments
            //{
            //    EndPointUrl= "https://localhost:8081",
            //    NameDb="agrodb",
            //    PrimaryKey= "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            //});


            //var orders = db.Orders.GetApplicationOrders();

            //var ordersDb = await new CommonDbOperations<ApplicationOrder>().TolistAsync(orders);

            //var search = new AgroSearch("agrisearch", "F9189208F49AF7C3DFD34E45A89F19E4");


            //var ordersSearch = ordersDb.Select(odb => new OrderSearch {
            //    Created = DateTime.Now,
            //    Name = odb.Name,
            //    OrderId = odb.Id
            //}).ToList();

            //search.AddOrders(ordersSearch);


            

        }
    }
}
