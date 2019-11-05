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

        public static BarrackOperations GetBarrackOperations(BarrackEnumIntances instance)
        {

            switch (instance)
            {
                case BarrackEnumIntances.DefaultInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumIntances.EmptyResultInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Empty).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumIntances.ExceptionInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Errors).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumIntances.PlotLandNullInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Nullables).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumIntances.VarietyNullInstance:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Nullables).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Values).Object,
                        It.IsAny<string>()
                        );
                case BarrackEnumIntances.SaveNewOrEditBarrack_Success:
                    return new BarrackOperations(
                        GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        PlotLandInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Barrack>.GetInstance(Results.Nullables).Object,
                        It.IsAny<string>()
                        );
            }
            throw new Exception("Bad parameters!");
        }
    }
}