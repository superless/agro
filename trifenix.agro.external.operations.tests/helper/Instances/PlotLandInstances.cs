using Moq;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class PlotLandInstances
    {
        public static Mock<IPlotLandRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IPlotLandRepository, PlotLand>(result,
                (s) => s.CreateUpdateSector(It.IsAny<PlotLand>()),
                (s) => s.GetPlotLand(It.IsAny<string>()),
                s => s.GetPlotLands());
    }
}
