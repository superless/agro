using System;
using trifenix.agro.common.tests.fakes;
using System.Linq;

namespace trifenix.agro.console
{
    class Program
    {
        static void Main(string[] args){
            var list = FakeGenerator.GetNotificationEvents().ToList();
            Console.WriteLine(list.Count);
            Console.WriteLine(FakeGenerator.GetNotificationEvent().Barrack);
            Console.WriteLine(FakeGenerator.CreateUpdateNotificationEvent());
        }
    }
}
