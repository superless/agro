using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class CertifiedEntitiesInstances
    {
        public static Mock<ICertifiedEntityRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<ICertifiedEntityRepository, CertifiedEntity>(result, (s) =>
                s.CreateUpdateCertifiedEntity(It.IsAny<CertifiedEntity>()),
                (s) => s.GetCertifiedEntity(It.IsAny<string>()),
                s => s.GetCertifiedEntities());
    }

    public static class ApplicationTargetInstances
    {
        public static Mock<IApplicationTargetRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IApplicationTargetRepository, ApplicationTarget>(result, (s) =>
                s.CreateUpdateTargetApp(It.IsAny<ApplicationTarget>()),
                (s) => s.GetTarget(It.IsAny<string>()),
                s => s.GetTargets());
    }
}
