using Moq;

namespace trifenix.agro.common.tests.interfaces
{

    public interface IMoqRepo<T> where T:class
    {
        Mock<T> GetMoqRepo(Results result);
    }

    public enum Results {
        Nullables,
        Empty,
        Errors,
        Values
    }
}
