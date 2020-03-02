using trifenix.agro.db.model;

namespace trifenix.agro.external.interfaces {
    public interface ICounterOperations {

        Counter GetCounter();

        int GetCorrelativePosition<T>(string specieAbb);

        void IncreaseCorrelativePosition<T>(string specieAbb);

        bool RemoveEntityFromCounter<T>();

        bool RemoveSpecieFromCounter(string specieAbb);

    }
}