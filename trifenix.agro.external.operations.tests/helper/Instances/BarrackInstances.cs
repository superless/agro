using Moq;
using System;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class BarrackInstances
    {
        public static Mock<IBarrackRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IBarrackRepository, Barrack>(result,
                (s) => s.CreateUpdateBarrack(It.IsAny<Barrack>()),
                (s) => s.GetBarrack(It.IsAny<string>()),
                s => s.GetBarracks());

        public static BarrackOperations GetBarrackOperations(BarrackEnumInstances instance)
        {

            switch (instance)
            {
                case BarrackEnumInstances.DefaultInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumInstances.EmptyResultInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Empty).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumInstances.ExceptionInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Errors).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumInstances.PlotLandNullInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Nullables).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumInstances.VarietyNullInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Nullables).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumInstances.SaveNewOrEditBarrack_Success:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Nullables).Object,
                        It.IsAny<string>()
                        );
            }
            throw new Exception("Bad parameters!");
        }
    }
}