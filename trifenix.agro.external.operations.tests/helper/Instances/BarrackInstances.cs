using Moq;
using System;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances {
    public static class BarrackInstances <T> where T : Barrack {
        public static Mock<IBarrackRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IBarrackRepository, Barrack>(result,
                (s) => s.CreateUpdateBarrack(It.IsAny<Barrack>()),
                (s) => s.GetBarrack(It.IsAny<string>()),
                s => s.GetBarracks());

        public static BarrackOperations<T> GetBarrackOperations(BarrackEnumInstances instance) {
            switch (instance) {
                case BarrackEnumInstances.DefaultInstance:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<T>.GetInstance(Results.Values).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
                case BarrackEnumInstances.EmptyResultInstance:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Empty).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<T>.GetInstance(Results.Values).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
                case BarrackEnumInstances.ExceptionInstance:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Errors).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<T>.GetInstance(Results.Values).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
                case BarrackEnumInstances.PlotLandNullInstance:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Nullables).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<T>.GetInstance(Results.Values).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
                case BarrackEnumInstances.VarietyNullInstance:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Nullables).Object,
                        CommonDbInstances<T>.GetInstance(Results.Values).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
                case BarrackEnumInstances.SaveNewOrEditBarrack_Success:
                    return new BarrackOperations<T>(
                        GetInstance(Results.Values).Object,
                        RootstockInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<T>.GetInstance(Results.Nullables).Object,
                        It.IsAny<string>(),
                        AgroSearchInstances.GetInstance().Object);
            }
            throw new Exception("Bad parameters!");
        }
    }
}