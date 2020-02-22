using trifenix.agro.db.model;

namespace trifenix.agro.external.interfaces {
    public interface ICounterOperations {

        //Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input);

        Counter GetCounter();

    }
}