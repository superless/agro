using AutoBogus;
using System.Linq;
using trifenix.agro.db;

namespace trifenix.agro.common.tests.fakes
{
    public static class FakeGenerator
    {
       


        public static IQueryable<T> GetElements<T>() where T:class
        {
            var list = AutoFaker.Create().Generate<T>(10);
            return list.AsQueryable();
        }

        public static T GetElement<T>() where T : class
        {
            return AutoFaker.Create().Generate<T>();

        }

        public static T GetElement<T>(string id) where T : DocumentBase
        {
            var element = AutoFaker.Create().Generate<T>();
            element.Id = id;
            return element;


        }

        public static string CreateString()
        {
            return AutoFaker.Create().Generate<string>();
        }



        

    }
}
