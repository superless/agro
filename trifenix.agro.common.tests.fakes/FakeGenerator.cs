using AutoBogus;
using System.Linq;
using trifenix.agro.db.model.agro;

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

        public static string CreateString()
        {
            return AutoFaker.Create().Generate<string>();
        }



        

    }
}
