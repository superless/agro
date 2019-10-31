using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.external.operations.tests.helper.Moqs.staticHelper;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqIngredientRepo : IMoqRepo<IIngredientRepository>
    {
        public Mock<IIngredientRepository> GetMoqRepo(Results result)
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqIngredient.GetBarrackWithResults();
                case Results.Empty:
                    return MoqIngredient.GetBarrackWithResults();
                case Results.Errors:
                    return MoqIngredient.GetBarrackWithResults();
                case Results.Values:
                    return MoqIngredient.GetBarrackWithResults();
                
            }
            throw new Exception("bad params");
        }
    }
}