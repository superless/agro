using AutoBogus;
using System.Linq;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.common.tests.fakes
{
    public static class FakeGenerator
    {
        public static IQueryable<NotificationEvent> GetNotificationEvents()
        {
            var list = AutoFaker.Create().Generate<NotificationEvent>(10);
            return list.AsQueryable();
        }

        public static NotificationEvent GetNotificationEvent()
        {
            return AutoFaker.Create().Generate<NotificationEvent>();
            
        }

        public static string CreateUpdateNotificationEvent()
        {
            return AutoFaker.Create().Generate<string>();
        }



        public static string CreateUpdateBarrack()
        {
            return AutoFaker.Create().Generate<string>();
        }

        public static Barrack GetBarrack()
        {
            return AutoFaker.Create().Generate<Barrack>();
        }

        public static IQueryable<Barrack> GetBarracks()
        {
            return AutoFaker.Create().Generate<Barrack>(10).AsQueryable();
        }



        public static string CreateUpdatePhenologicalEvent()
        {
            return AutoFaker.Create().Generate<string>();
        }

        public static PhenologicalEvent GetPhenologicalEvent()
        {
            return AutoFaker.Create().Generate<PhenologicalEvent>();
        }

        public static IQueryable<PhenologicalEvent> GetPhenologicalEvents()
        {
            return AutoFaker.Create().Generate<PhenologicalEvent>(10).AsQueryable();
        }



        public static string UploadImageBase64()
        {
            return AutoFaker.Create().Generate<string>();
        }

    }
}
